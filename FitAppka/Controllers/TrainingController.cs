using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Service;
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
        private readonly FitAppContext _context;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICardioTrainingService _cardioServices;
        private readonly IStrengthTrainingService _strengthTrainingService;

        public TrainingController(IClientRepository clientRepository, ICardioTrainingService cardioTrainingServices, 
            IStrengthTrainingService strengthTrainingServices, FitAppContext context, IDayRepository dayRepository) 
        {
            _dayRepository = dayRepository;
            _context = context;
            _clientRepository = clientRepository;
            _cardioServices = cardioTrainingServices;
            _strengthTrainingService = strengthTrainingServices;
        }

        [HttpGet]
        public async Task<IActionResult> TrainingPanel(int dayID) {

            ViewData["dayID"] = dayID;
            ViewData["day"] = _dayRepository.GetDayDateTime(dayID).Date.ToString("dd.MM.yyyy");
            ViewData["clientID"] = _clientRepository.GetLoggedInClient().ClientId;
            ViewData["burnedKcal"] = _cardioServices.CaloriesBurnedInDay(dayID);
            ViewData["cardioTime"] = _cardioServices.CardioTimeInDay(dayID);
            ViewData["kcalTarget"] = _cardioServices.GetKcalBurnedGoalInDay(dayID);
            ViewData["timeTarget"] = _cardioServices.GetTrainingTimeGoalInDay(dayID);
            ViewData["strengthTrainings"] = _context.StrengthTraining.Include(s => s.StrengthTrainingType).Include(s => s.Day).ToList();

            return View(await _context.CardioTraining.Include(c => c.CardioTrainingType).Include(c => c.Day).ToListAsync());
        }

        [HttpGet]
        public IActionResult ChangeDay(string day) {
            return RedirectToAction(nameof(TrainingPanel), new { dayID = _cardioServices.GetSelectedDay(Convert.ToDateTime(day)) });
        }

        [HttpGet]
        public async Task<IActionResult> Cardio(string search, int dayID) {
            ViewData["dayID"] = dayID;
            CheckIfThisWasSearchedFor(search);
            return View(await _context.CardioTrainingType.Where(c => c.TrainingName.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult Cardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal) {
            try 
            {
                _cardioServices.AddCardio(cardioTypeId, dayID, timeInMinutes, burnedKcal);
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = _cardioServices.GetTodayID() }); } 
        }

        [HttpPut]
        public JsonResult Cardio(int id, int time, int burnedKcal)
        {
            return Json(_cardioServices.EditCardio(id, time, burnedKcal));
        }

        [HttpDelete]
        public JsonResult Cardio(int id)
        {
            return Json(_cardioServices.DeleteCardio(id));
        }

        [HttpGet]
        public async Task<IActionResult> StrengthTraining(string search, int dayID) {
            ViewData["dayID"] = dayID;
            CheckIfThisWasSearchedFor(search);
            return View(await _context.StrengthTrainingType.Where(s => s.TrainingName.Contains(search)).ToListAsync());
        }

        [HttpPost]
        public IActionResult StrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight) {
            try 
            {
                _strengthTrainingService.AddStrengthTraining(trainingTypeId, dayID, sets, reps, weight);
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = _cardioServices.GetTodayID() }); } 
        }

        [HttpPut]
        public JsonResult StrengthTraining(int id, short sets, short reps, short weight)
        {
            return Json(_strengthTrainingService.EditStrengthTraining(id, sets, reps, weight));
        }

        [HttpDelete]
        public JsonResult StrengthTraining(int id)
        {
            return Json(_strengthTrainingService.DeleteStrengthTraining(id));
        }

        [HttpPost]
        public IActionResult AddCardioType(int dayID, string name, int kcalPerMin)
        {
            _cardioServices.AddCardioTrainingType(dayID, name, kcalPerMin);
            return RedirectToAction(nameof(Cardio), new { dayID });
        }

        [HttpPost]
        public IActionResult AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight)
        {
            _strengthTrainingService.AddStrengthTrainingType(dayID, name, sets, reps, weight);
            return RedirectToAction(nameof(TrainingPanel), new { dayID });
        }

        private void CheckIfThisWasSearchedFor(string search) {
            ViewData["wasSearched"] = false;
        
            if (search != null)
            {
                ViewData["wasSearched"] = true;
            }
        }
    }
}
