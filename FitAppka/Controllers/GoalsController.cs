using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitAppka.Repository;
using FitAppka.Models;
using FitAppka.Service;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IDayManageService _dayService;
        private readonly IClientRepository _clientRepository;
        private readonly IGoalsService _goalsService;

        public GoalsController(IDayManageService dayService, IClientRepository clientRepository, IGoalsService goalsService)
        {
            _dayService = dayService;
            _goalsService = goalsService;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult Goals()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            SendDataAboutGoals();
            return View();
        }

        [HttpPost]
        public IActionResult Goals(CreateGoalsModel model)
        {
            if (ModelState.IsValid) {
                _goalsService.UpdateGoals(model);
                return RedirectToAction("Start", "Home");
            }
            else {
                SendDataAboutGoals();
                return View(model);
            }
        }

        private void SendDataAboutGoals()
        {
            Client client = _clientRepository.GetLoggedInClient();
            ViewData["auto"] = client.AutoDietaryGoals;
            ViewData["include"] = client.IncludeCaloriesBurned;
            ViewData["burnGoal"] = client.KcalBurnedGoal;
            ViewData["timeGoal"] = client.TrainingTimeGoal;
            ViewData["calorieTarget"] = client.CalorieGoal;
            ViewData["proteinTarget"] = client.ProteinTarget;
            ViewData["fatTarget"] = client.FatTarget;
            ViewData["carbsTarget"] = client.CarbsTarget;

        }

    }
}
