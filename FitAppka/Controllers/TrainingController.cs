using FitnessApp.Models;
using FitnessApp.Repository;
using FitnessApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class TrainingController : Controller
    {
        private readonly FitAppContext _context;
        private readonly ICardioTrainingService _cardioServices;
        private readonly IStrengthTrainingService _strengthTrainingService;
        private readonly ITrainingPanelService _trainingPanelService;
        //private readonly IGoalsService _goalsService;
        private readonly IDayManageService _dayService;

        public TrainingController(ICardioTrainingService cardioTrainingServices, IStrengthTrainingService strengthTrainingServices, 
            IDayManageService dayService, ITrainingPanelService trainingPanelService) 
        {
            _trainingPanelService = trainingPanelService;
            _dayService = dayService;
            _cardioServices = cardioTrainingServices;
            _strengthTrainingService = strengthTrainingServices;
        }

        [HttpGet]
        public async Task<IActionResult> Cardio(string search, int dayID)
        {
            ViewData["dayID"] = dayID;
            ViewData["wasSearched"] = search != null;
            return View(await _cardioServices.GetCardioTrainingTypes(search));
        }

        [HttpPost]
        public IActionResult Cardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal)
        {
            try {
                _cardioServices.AddCardio(cardioTypeId, dayID, timeInMinutes, burnedKcal);
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = _dayService.GetTodayId() }); }
        }

        [HttpPut]
        public JsonResult Cardio(int id, int time, int burnedKcal) {
            return Json(_cardioServices.EditCardio(id, time, burnedKcal));
        }

        [HttpDelete]
        public JsonResult Cardio(int id) {
            return Json(_cardioServices.DeleteCardio(id));
        }


        [HttpGet]
        [Route("/Training")]
        public IActionResult TrainingPanel(int dayID) {

            //  ViewData["dayID"] = dayID;
            //  ViewData["day"] = _dayService.GetDayDateTime(dayID).Date.ToString("dd.MM.yyyy");
            //  ViewData["clientID"] = _clientRepository.GetLoggedInClient().ClientId;
            //  ViewData["burnedKcal"] = _goalsService.CaloriesBurnedInDay(dayID);
            //  ViewData["cardioTime"] = _cardioServices.GetCardioTimeInDay(dayID);
            //  ViewData["kcalTarget"] = _cardioServices.GetKcalBurnedGoalInDay(dayID);
            //  ViewData["timeTarget"] = _cardioServices.GetTrainingTimeGoalInDay(dayID);
            //  ViewData["strengthTrainings"] = _context.StrengthTraining.Include(s => s.StrengthTrainingType).Include(s => s.Day).ToList();

            return View(_trainingPanelService.Dto(dayID));//await _context.CardioTraining.Include(c => c.CardioTrainingType).Include(c => c.Day).ToListAsync());
        }

        [HttpGet]
        public IActionResult ChangeDay(string day) {
            return RedirectToAction(nameof(TrainingPanel), new { dayID = _dayService.GetDayIDByDate(Convert.ToDateTime(day)) });
        }

        

        [HttpGet]
        public async Task<IActionResult> StrengthTraining(string search, int dayID) {
            ViewData["dayID"] = dayID;
            ViewData["wasSearched"] = search != null;
            return View(await _strengthTrainingService.GetStrengthTrainingTypes(search));
        }

        [HttpPost]
        public IActionResult StrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight) {
            try 
            {
                _strengthTrainingService.AddStrengthTraining(trainingTypeId, dayID, sets, reps, weight);
                return RedirectToAction(nameof(TrainingPanel), new { dayID });
            }
            catch { return RedirectToAction(nameof(TrainingPanel), new { dayID = _dayService.GetTodayId() }); } 
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
        public JsonResult AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight)
        {
            _strengthTrainingService.AddStrengthTrainingType(dayID, name, sets, reps, weight);
            return Json(new { redirect = Url.Action("TrainingPanel", "Training", new { dayID })});
        }

    }
}
