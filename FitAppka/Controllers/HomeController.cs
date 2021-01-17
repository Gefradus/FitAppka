using System;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Repository;
using FitnessApp.Service;
using FitnessApp.Service.ServiceImpl;

namespace NowyDotnecik.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IDayManageService _dayService;
        private readonly IClientRepository _clientRepository;
        private readonly IHomePageService _homeService;
        private readonly IContentRootPathHandlerService _contentRootService;

        public HomeController(FitAppContext context, IHomePageService homePageService, 
            IDayManageService dayService, IClientRepository clientRepository, IContentRootPathHandlerService contentRootService)
        {
            _contentRootService = contentRootService;
            _homeService = homePageService;
            _dayService = dayService;
            _clientRepository = clientRepository;
            _context = context;
        }

        [HttpGet]
        [Route("/Home")]
        public IActionResult Home(DateTime daySelected)
        {
            _dayService.AddDayIfNotExists(daySelected);
            SendInfoAboutMacronutritions(_dayService.GetLoggedInClientDayByDate(daySelected));

            return View(_homeService.CreateHomeDTO(daySelected));
        }

        [HttpGet]
        public IActionResult Start()
        {
            if (_homeService.IsItFirstLaunch()) {
                return RedirectToAction("Settings", "Settings");
            }
            return RedirectToAction(nameof(Home), new { daySelected = DateTime.Now.Date});
        }


        [HttpGet]
        public IActionResult Return(int dayID)
        {
            DateTime day = _dayService.GetDayDateTime(dayID);
            DateTime daySelected = day != null && dayID != 0 ? day : DateTime.Now.Date;

            return RedirectToAction(nameof(Home), new { daySelected });
        }

        [HttpGet]
        public IActionResult ChooseDay(int choice, int dayID)
        {
            DateTime daySelected = _dayService.GetDayDateTime(dayID);
            return RedirectToAction(nameof(Home), new { daySelected = choice == 1 ? daySelected.AddDays(-1) : daySelected.AddDays(1) });
        }

        [HttpGet]
        public IActionResult ChangeDay(string day) 
        { 
            return RedirectToAction(nameof(Home), new { daySelected = Convert.ToDateTime(day) });
        }

        [HttpPost]
        [ActionName("Water")]
        public JsonResult AddWater(int dayID, int addedWater)
        {
            _homeService.AddWater(dayID, addedWater);
            return Json(true);
        }

        [HttpPut]
        [ActionName("Water")]
        public JsonResult EditWater(int dayID, int editedWater)
        {
            _homeService.EditWater(dayID, editedWater);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Meal(int atWhichMealOfTheDay, int dayID, int grammage, int productID)
        {
            DateTime daySelected = _homeService.AddMeal(atWhichMealOfTheDay, dayID, grammage, productID);
            return Json(new { redirectToUrl = Url.Action("Home","Home", new { daySelected }) });
        }

        [HttpPut]
        public JsonResult Meal(int id, int grammage)
        {
            _homeService.EditMeal(id, grammage);
            return Json(true);
        }

        [HttpDelete]
        public JsonResult Meal(int id)
        {
            _homeService.DeleteMeal(id);
            return Json(true);
        }

       
        private void SendInfoAboutMacronutritions(Day day)
        {
            Goals dayGoals = _dayService.GetDayGoals(day.DayId);
            CountMacronutrionNumbers(_homeService.SumAllKcalInDay((DateTime)day.Date), "calories", dayGoals.Calories);
            CountMacronutrionNumbers(_homeService.SumAllProteinsInDay((DateTime)day.Date), "proteins", dayGoals.Proteins);
            CountMacronutrionNumbers(_homeService.SumAllCarbsInDay((DateTime)day.Date), "carbs", dayGoals.Carbohydrates);
            CountMacronutrionNumbers(_homeService.SumAllFatsInDay((DateTime)day.Date), "fats", dayGoals.Fats);
        }

        private void CountMacronutrionNumbers(double sumOfMacronutrion, string name, double target)
        {
            ViewData[name] = sumOfMacronutrion;
            ViewData[name + "0"] = _homeService.Round(sumOfMacronutrion);
            ViewData[name + "Target"] = target;
            ViewData["%" + name] = _homeService.CountPercentageOfTarget(sumOfMacronutrion, target);
        }

    }
}
