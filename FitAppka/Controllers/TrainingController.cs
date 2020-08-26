using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class TrainingController : Controller
    {
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;

        public TrainingController(FitAppContext context, IDayRepository dayRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TrainingPanel(int dayID)
        {
            dayID = GiveTodayIfDayNotChosen(dayID);
            ViewData["day"] = GetDayDateById(dayID);
            ViewData["dayID"] = dayID;
            ViewData["clientID"] = DajZalogowanegoKlientaID();
            ViewData["burnedKcal"] = CaloriesBurnedInDay(dayID);
            ViewData["cardioTime"] = CardioTimeInDay(dayID);
            ViewData["kcalTarget"] = GetKcalBurnedGoalInDay(dayID);
            ViewData["timeTarget"] = GetTrainingTimeGoalInDay(dayID);

            return View(await _context.CardioTraining.ToListAsync());
        }

        [HttpGet]
        public IActionResult ChangeDay(string day)
        {
            return RedirectToAction(nameof(TrainingPanel), new { dzienID = GetSelectedDay(Convert.ToDateTime(day)) });
        }

        private int GetKcalBurnedGoalInDay(int dayID)
        {
            int? kcalBurnedGoal = _dayRepository.GetDay(dayID).KcalBurnedGoal;
            if(kcalBurnedGoal != null)
            {
                return (int) kcalBurnedGoal;
            }
            else
            {
                return 0;
            }

        }

        private int GetTrainingTimeGoalInDay(int dayID)
        {
            int? celMinut = _dayRepository.GetDay(dayID).TrainingTimeGoal;
            if (celMinut != null)
            {
                return (int)celMinut;
            }
            else
            {
                return 0;
            }
        }



        private int CaloriesBurnedInDay(int dayID)
        {
            List<CardioTraining> trainings = _context.CardioTraining.Where(t => t.DayId.Equals(dayID)).ToList();
            int? kcal = 0;
            foreach (CardioTraining cardio in trainings) {
                kcal += cardio.CaloriesBurned;
            }

            return (int) kcal;
        }

        private int GiveTodayIfDayNotChosen(int dzienID)
        {
            if(dzienID == 0)
            {
                return GetTodayID();
            }
            else
            {
                return dzienID;
            }
        }

        private int CardioTimeInDay(int dayID)
        {
            List<CardioTraining> trainings = _context.CardioTraining.Where(t => t.DayId.Equals(dayID)).ToList();
            int? time = 0;
            foreach (CardioTraining cardio in trainings)
            {
                time += cardio.TimeInMinutes;
            }

            return (int)time;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search, int dayID)
        {
            ViewData["dayID"] = dayID;
            CheckIfThisWasSearchedFor(search);
            return View(await _context.CardioTrainingType.Where(c => c.TrainingName.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(int dayID, int cardioTypeId, int timeInMinutes, int burnedKcal)
        {
            _context.Add(new CardioTraining()
            {
                DayId = dayID,
                TimeInMinutes = timeInMinutes,
                CardioTrainingTypeId = cardioTypeId,
                CaloriesBurned = burnedKcal
            });

            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = GetTodayID() }); } 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCardio(int cardioID)
        {
            var training = _context.CardioTraining.Where(t => t.CardioTrainingId == cardioID).FirstOrDefault();
           
            if (training != null)
            {
                _context.CardioTraining.Remove(training);
                await _context.SaveChangesAsync();
            }
            return Json(false);
        }

        [HttpPost]
        public IActionResult AddTraining(int dzienID, string nazwa, int wydatek)
        {
            _context.Add(new CardioTrainingType
            {
                TrainingName = nazwa,
                KcalPerMin = wydatek
            });

            _context.SaveChanges();

            return RedirectToAction(nameof(Search), new { dzienID });
        }


        private int GetSelectedDay(DateTime day)
        {
            AddDayIfNotExists(day);
            return GetClientDayIDByDate(day);
        }

        private void AddDayIfNotExists(DateTime day)
        {
            var client = _clientRepository.GetClientById(DajZalogowanegoKlientaID());
            int count = _context.Day.Count(dz => dz.Date == day && dz.ClientId == client.ClientId);
            if (count == 0)
            {
                _dayRepository.Add(new Day()
                {
                    Date = day,
                    ClientId = DajZalogowanegoKlientaID(),
                    Breakfast = client.Breakfast,
                    Lunch = client.Lunch,
                    Dinner = client.Dinner,
                    Dessert = client.Dessert,
                    Snack = client.Snack,
                    Supper = client.Supper,
                    ProteinTarget = client.ProteinTarget,
                    FatTarget = client.FatTarget,
                    CarbsTarget = client.CarbsTarget,
                    CalorieTarget = client.CarbsTarget,
                    WaterDrunk = 0,
                });
            }
        }


        private int GetClientDayIDByDate(DateTime day)
        {
            AddDayIfNotExists(day);
            return _dayRepository.GetClientDayByDate(day, DajZalogowanegoKlientaID()).DayId;
        }


        private int GetTodayID()
        {
            return GetSelectedDay(DateTime.Now);
        }

        private string GetDayDateById(int dayID)
        {
            return _dayRepository.GetDayDateTime(dayID).Date.ToString("dd.MM.yyyy");
        }

        private void CheckIfThisWasSearchedFor(string search){
            ViewData["wasSearched"] = false;
        
            if (search != null)
            {
                ViewData["wasSearched"] = true;
            }
        }

        private int DajZalogowanegoKlientaID() {
            return _clientRepository.GetClientByLogin(User.Identity.Name).ClientId;
        }
    }
}
