using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly IStrengthTrainingTypeRepository _strengthTrainingTypeRepository;
        private readonly IStrengthTrainingRepository _strengthTrainingRepository;
        private readonly FitAppContext _context;

        public TrainingController(FitAppContext context, IDayRepository dayRepository, 
            IClientRepository clientRepository, ICardioTrainingTypeRepository cardioTypeRepository, 
            ICardioTrainingRepository cardioRepository, IStrengthTrainingTypeRepository strengthTrainingTypeRepository, 
            IStrengthTrainingRepository strengthTrainingRepository) 
        {
            _cardioTypeRepository = cardioTypeRepository;
            _cardioRepository = cardioRepository;
            _strengthTrainingTypeRepository = strengthTrainingTypeRepository;
            _strengthTrainingRepository = strengthTrainingRepository;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TrainingPanel(int dayID) {
            dayID = GiveTodayIfDayNotChosen(dayID);
            ViewData["day"] = GetDayDateById(dayID);
            ViewData["dayID"] = dayID;
            ViewData["clientID"] = GetLoggedInClientID();
            ViewData["burnedKcal"] = CaloriesBurnedInDay(dayID);
            ViewData["cardioTime"] = CardioTimeInDay(dayID);
            ViewData["kcalTarget"] = GetKcalBurnedGoalInDay(dayID);
            ViewData["timeTarget"] = GetTrainingTimeGoalInDay(dayID);
            ViewData["strengthTrainings"] = _context.StrengthTraining.Include(s => s.StrengthTrainingType).Include(s => s.Day).ToList();

            return View(await _context.CardioTraining.Include(c => c.CardioTrainingType).Include(c => c.Day).ToListAsync());
        }


        [HttpGet]
        public IActionResult ChangeDay(string day) {
            return RedirectToAction(nameof(TrainingPanel), new { dayID = GetSelectedDay(Convert.ToDateTime(day)) });
        }

        private int GetKcalBurnedGoalInDay(int dayID) {
            int? kcalBurnedGoal = _dayRepository.GetDay(dayID).KcalBurnedGoal;
            if(kcalBurnedGoal != null) {
                return (int) kcalBurnedGoal;
            }
            else {
                return 0;
            }
        }

        private int GetTrainingTimeGoalInDay(int dayID) {
            int? timeGoal = _dayRepository.GetDay(dayID).TrainingTimeGoal;
            if (timeGoal != null) {
                return (int)timeGoal;
            }
            else {
                return 0;
            }
        }

        private int CaloriesBurnedInDay(int dayID) {
            int? kcal = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID))) {
                kcal += cardio.CaloriesBurned;
            }

            return (int) kcal;
        }

        private int GiveTodayIfDayNotChosen(int dayID) {
            if(dayID == 0) {
                return GetTodayID();
            }
            else {
                return dayID;
            }
        }

        private int CardioTimeInDay(int dayID) {
            int? time = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID))) {
                time += cardio.TimeInMinutes;
            }

            return (int)time;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search, int dayID) {
            ViewData["dayID"] = dayID;
            CheckIfThisWasSearchedFor(search);
            return View(await _context.CardioTrainingType.Where(c => c.TrainingName.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult Search(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal) {
            try {
                _cardioRepository.Add(new CardioTraining() {
                    DayId = dayID,
                    TimeInMinutes = timeInMinutes,
                    CardioTrainingTypeId = cardioTypeId,
                    CaloriesBurned = burnedKcal
                });
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = GetTodayID() }); } 
        }

        [HttpPost]
        public JsonResult DeleteCardio(int cardioID) {
            _cardioRepository.Delete(cardioID);
            return Json(false);
        }

        [HttpPost]
        public IActionResult AddCardioType(int dayID, string name, int kcalPerMin) {
            _cardioTypeRepository.Add(new CardioTrainingType {
                TrainingName = name,
                KcalPerMin = kcalPerMin
            });

            return RedirectToAction(nameof(Search), new { dayID });
        }

        private int GetSelectedDay(DateTime day) {
            AddDayIfNotExists(day);
            return GetClientDayIDByDate(day);
        }

        private void AddDayIfNotExists(DateTime day) {
            var client = _clientRepository.GetClientById(GetLoggedInClientID());
            int count = _context.Day.Count(dz => dz.Date == day && dz.ClientId == client.ClientId);
            if (count == 0)
            {
                _dayRepository.Add(new Day() {
                    Date = day,
                    ClientId = client.ClientId,
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


        private int GetClientDayIDByDate(DateTime day) {
            AddDayIfNotExists(day);
            return _dayRepository.GetClientDayByDate(day, GetLoggedInClientID()).DayId;
        }

        private int GetTodayID() {
            return GetSelectedDay(DateTime.Now);
        }

        private string GetDayDateById(int dayID) {
            return _dayRepository.GetDayDateTime(dayID).Date.ToString("dd.MM.yyyy");
        }

        private void CheckIfThisWasSearchedFor(string search) {
            ViewData["wasSearched"] = false;
        
            if (search != null)
            {
                ViewData["wasSearched"] = true;
            }
        }

        private int GetLoggedInClientID() {
            return _clientRepository.GetClientByLogin(User.Identity.Name).ClientId;
        }
    }
}
