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
        public async Task<IActionResult> TrainingPanel(int dzienID)
        {
            dzienID = DajDzisJesliNieWybranoDnia(dzienID);
            ViewData["dzien"] = DajDzienPoID(dzienID);
            ViewData["dzienID"] = dzienID;
            ViewData["klientID"] = DajZalogowanegoKlientaID();
            ViewData["spaloneKcal"] = CaloriesBurnedInDay(dzienID);
            ViewData["czasTreningow"] = CzasTreningowWDniu(dzienID);
            ViewData["celKcal"] = GetKcalBurnedGoalInDay(dzienID);
            ViewData["celMinut"] = GetTrainingTimeGoalInDay(dzienID);
            return View(await _context.CardioTraining.Include(t => t.CardioTrainingType).Include(t => t.DayId).ToListAsync());
        }

        [HttpGet]
        public IActionResult ChangeDay(string day)
        {
            return RedirectToAction(nameof(TrainingPanel), new { dzienID = DajObecnyDzienID(Convert.ToDateTime(day)) });
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

        private int DajDzisJesliNieWybranoDnia(int dzienID)
        {
            if(dzienID == 0)
            {
                return DajDzisiajID();
            }
            else
            {
                return dzienID;
            }
        }

        private int CzasTreningowWDniu(int dayID)
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
        public async Task<IActionResult> Search(string search, int dzienID)
        {
            ViewData["dzienID"] = dzienID;
            SprawdzCzySzukano(search);
            return View(await _context.CardioTrainingType.Where(c => c.TrainingName.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(int dzienID, int treningTypId, int czasWMinutach, int spaloneKcal)
        {
            _context.Add(new CardioTraining()
            {
                DayId = dzienID,
                TimeInMinutes = czasWMinutach,
                CardioTrainingTypeId = treningTypId,
                CaloriesBurned = spaloneKcal
            });

            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(TrainingPanel), new { dzienID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dzienID = DajDzisiajID() }); } 
        }

        [HttpPost]
        public async Task<IActionResult> UsunTrening(int treningID)
        {
            var trening = _context.CardioTraining.Where(t => t.CardioTrainingId == treningID).FirstOrDefault();
           
            if (trening != null)
            {
                _context.CardioTraining.Remove(trening);
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


        private int DajObecnyDzienID(DateTime dzien)
        {
            int id = GetClientDayIDByDate(dzien);
            if (id != 0)
            {
                return id;
            }
            else
            {
                DodajNowyDzien(dzien);
                return GetClientDayIDByDate(dzien);
            }

        }

        private void DodajNowyDzien(DateTime day)
        {
            var client = _clientRepository.GetClient(DajZalogowanegoKlientaID());

            _dayRepository.Add(new Day() {
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
            }); ;
        }


        private int GetClientDayIDByDate(DateTime day)
        {
            return _dayRepository.GetClientDayByDate(day, DajZalogowanegoKlientaID()).DayId;
        }


        private int DajDzisiajID()
        {
            return DajObecnyDzienID(DateTime.Now);
        }

        private string DajDzienPoID(int dzienID)
        {
            DateTime dateTime = (DateTime) _context.Day.Where(d => d.DayId == dzienID && d.ClientId == DajZalogowanegoKlientaID()).Select(d => d.Date).FirstOrDefault();
            return dateTime.Date.ToString("dd.MM.yyyy");
        }

        private void SprawdzCzySzukano(string search){
            ViewData["czySzukano"] = false;
        
            if (search != null)
            {
                ViewData["czySzukano"] = true;
            }
        }

        private int DajZalogowanegoKlientaID(){
            return _context.Client.Where(k => k.Login.ToLower() == User.Identity.Name.ToLower()).Select(k => k.ClientId).FirstOrDefault();
        }


        private string NazwaPosilku(int wKtorym)
        {
            if (wKtorym == 1) { return "Śniadanie"; }
            if (wKtorym == 2) { return "II śniadanie"; }
            if (wKtorym == 3) { return "Obiad"; }
            if (wKtorym == 4) { return "Deser"; }
            if (wKtorym == 5) { return "Przekąska"; }
            return "Kolacja";
        }

    }
}
