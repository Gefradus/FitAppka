using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using FitAppka.Repository;
using FitAppka.Service;

namespace NowyDotnecik.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDayManageService _dayService;
        private readonly IClientRepository _clientRepository;
        private readonly IHomePageService _homeService;

        public HomeController(FitAppContext context, IWebHostEnvironment env, IHomePageService homePageService,
            IDayManageService dayService, IClientRepository clientRepository)
        {
            _homeService = homePageService;
            _dayService = dayService;
            _clientRepository = clientRepository;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Home(DateTime daySelected)
        {
            _dayService.AddDayIfNotExists(daySelected);
            SendAllDataToView(daySelected);
            return View(await _context.Meal.Include(m => m.Day).Include(p => p.Product).
                Where(m => m.DayId == _dayService.GetLoggedInClientDayByDate(daySelected).DayId).ToListAsync());
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

            if (choice == 1) {
                return RedirectToAction(nameof(Home), new { daySelected = daySelected.AddDays(-1) });
            }
            else {
                return RedirectToAction(nameof(Home), new { daySelected = daySelected.AddDays(1) });
            }
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

        private void SendAllDataToView(DateTime daySelected)
        {
            Day day = _dayService.GetLoggedInClientDayByDate(daySelected);

            SendBasicData(daySelected, day);
            SendInfoIfIsAdminClient();
            SendInfoAboutMacronutritions(day);
            SendInfoAboutMealsOfTheDay(day);
            SendInfoAboutCaloriesInMeals(daySelected);
        }

        private void SendBasicData(DateTime daySelected, Day day)
        {
            ViewData["day"] = daySelected;
            ViewData["date"] = _homeService.DateFormat(daySelected);
            ViewData["datepick"] = daySelected.ToString("yyyy-MM-dd");
            ViewData["path"] = _env.WebRootPath.ToString();
            ViewData["clientID"] = _clientRepository.GetLoggedInClientId();
            ViewData["dayID"] = day.DayId;
            ViewData["water"] = day.WaterDrunk;
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

        private void SendInfoAboutMealsOfTheDay(Day day)
        {
            ViewData["breakfast"] = day.Breakfast;
            ViewData["lunch"] = day.Lunch;
            ViewData["dinner"] = day.Dinner;
            ViewData["dessert"] = day.Dessert;
            ViewData["snack"] = day.Snack;
            ViewData["supper"] = day.Supper;
        }

        private void SendInfoAboutCaloriesInMeals(DateTime daySelected)
        {
            ViewData["breakfastKcal"] = _homeService.CountCalories(1, daySelected);
            ViewData["lunchKcal"] = _homeService.CountCalories(2, daySelected);
            ViewData["dinnerKcal"] = _homeService.CountCalories(3, daySelected);
            ViewData["dessertKcal"] = _homeService.CountCalories(4, daySelected);
            ViewData["snackKcal"] = _homeService.CountCalories(5, daySelected);
            ViewData["supperKcal"] = _homeService.CountCalories(6, daySelected);
        }

        private void SendInfoIfIsAdminClient()
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                ViewData["admin"] = 1;
            }
            else {
                ViewData["admin"] = 0;
            }
        }

    }
}
