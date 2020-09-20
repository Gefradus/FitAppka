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
        private readonly IGoalsRepository _goalsRepository;

        public GoalsController(IDayManageService dayService, IClientRepository clientRepository, 
            IGoalsService goalsService, IGoalsRepository goalsRepository)
        {
            _goalsRepository = goalsRepository;
            _dayService = dayService;
            _goalsService = goalsService;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public IActionResult Goals()
        {
            ViewData["dayID"] = _dayService.GetTodayId();
            SendDataAboutGoals();
            return View(SendDataAboutGoals());
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

        private CreateGoalsModel SendDataAboutGoals()
        {
            Client client = _clientRepository.GetLoggedInClient();
            Goals clientGoals = _goalsRepository.GetClientGoals(client.ClientId);

            return new CreateGoalsModel()
            {
                AutoDietaryGoals = (bool)client.AutoDietaryGoals,
                IncludeCaloriesBurned = (bool)client.IncludeCaloriesBurned,
                KcalBurnedGoal = clientGoals.KcalBurned,
                TrainingTimeGoal = clientGoals.TrainingTime,
                CalorieGoal = (int?)clientGoals.Calories,
                ProteinTarget = (int?)clientGoals.Proteins,
                FatTarget = (int?)clientGoals.Fats,
                CarbsTarget = (int?)clientGoals.Carbohydrates,
                VitaminA = clientGoals.VitaminA,
                VitaminB1 = clientGoals.VitaminB1,
                VitaminB2 = clientGoals.VitaminB2,
                VitaminB5 = clientGoals.VitaminB5,
                VitaminB6 = clientGoals.VitaminB6,
                VitaminB12 = clientGoals.VitaminB12,
                VitaminC = clientGoals.VitaminC,
                VitaminD = clientGoals.VitaminD,
                VitaminE = clientGoals.VitaminE,
                VitaminK = clientGoals.VitaminK,
                VitaminPp = clientGoals.VitaminPp,
                Biotin = clientGoals.Biotin,
                Calcium = clientGoals.Calcium,
                Copper = clientGoals.Copper,
                FolicAcid = clientGoals.FolicAcid,
                Iodine = clientGoals.Iodine,
                Iron = clientGoals.Iron,
                Magnesium = clientGoals.Magnesium,
                Phosphorus = clientGoals.Phosphorus,
                Potassium = clientGoals.Potassium,
                Selenium = clientGoals.Selenium,
                Sodium = clientGoals.Sodium,
                Zinc = clientGoals.Zinc,
            };
        }

    }
}
