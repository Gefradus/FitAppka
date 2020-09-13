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
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHomePageService _homeService;

        public HomeController(FitAppContext context, IWebHostEnvironment env, IHomePageService homePageService,
            IDayRepository dayRepository, IClientRepository clientRepository)
        {
            _homeService = homePageService;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Home(DateTime daySelected)
        {
            _homeService.Home(daySelected);
            SendAllDataToView(daySelected);
            return View(await _context.Meal.Include(m => m.Day).Include(p => p.Product).
                Where(m => m.DayId == _dayRepository.GetClientDayByDate(daySelected, _clientRepository.GetLoggedInClientId()).DayId).ToListAsync());
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
            DateTime daySelected;
            DateTime day = _dayRepository.GetDayDateTime(dayID);
            if (day != null && dayID != 0) { 
                daySelected = day; 
            }
            else { 
                daySelected = DateTime.Now.Date; 
            }
  
            return RedirectToAction(nameof(Home), new { daySelected });
        }

        [HttpGet]
        public IActionResult ChooseDay(int choice, int dayID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);

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
        public JsonResult Meal(int inWhich, int dayID, int grammage, int productID)
        {
            _homeService.AddMeal(inWhich, dayID, grammage, productID);
            return Json(true);
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
            int clientID = _clientRepository.GetLoggedInClientId();
            Day day = _dayRepository.GetClientDayByDate(daySelected, clientID);

            SendBasicData(daySelected, clientID, day);
            SendInfoIfIsAdminClient(clientID);
            SendInfoAboutMacronutritions(day);
            SendInfoAboutMealsOfTheDay(day);
            SendInfoAboutCaloriesInMeals(daySelected, clientID);
        }

        private void SendBasicData(DateTime daySelected, int clientID, Day day)
        {
            ViewData["day"] = daySelected;
            ViewData["date"] = _homeService.DateFormat(daySelected);
            ViewData["datepick"] = daySelected.ToString("yyyy-MM-dd");
            ViewData["path"] = _env.WebRootPath.ToString();
            ViewData["clientID"] = clientID;
            ViewData["dayID"] = day.DayId;
            ViewData["water"] = day.WaterDrunk;
        }

        private void SendInfoAboutMacronutritions(Day day)
        {
            CountMacronutrionNumbers(_homeService.SumAllKcalInDay((DateTime)day.Date), "calories", day.CalorieGoal);
            CountMacronutrionNumbers(_homeService.SumAllProteinsInDay((DateTime)day.Date), "proteins", day.ProteinTarget);
            CountMacronutrionNumbers(_homeService.SumAllCarbsInDay((DateTime)day.Date), "carbs", day.CarbsTarget);
            CountMacronutrionNumbers(_homeService.SumAllFatsInDay((DateTime)day.Date), "fats", day.FatTarget);
        }

        private void CountMacronutrionNumbers(double sumOfMacronutrion, string name, int? target)
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

        private void SendInfoAboutCaloriesInMeals(DateTime daySelected, int clientID)
        {
            ViewData["breakfastKcal"] = _homeService.CountCalories(1, daySelected, clientID);
            ViewData["lunchKcal"] = _homeService.CountCalories(2, daySelected, clientID);
            ViewData["dinnerKcal"] = _homeService.CountCalories(3, daySelected, clientID);
            ViewData["dessertKcal"] = _homeService.CountCalories(4, daySelected, clientID);
            ViewData["snackKcal"] = _homeService.CountCalories(5, daySelected, clientID);
            ViewData["supperKcal"] = _homeService.CountCalories(6, daySelected, clientID);
        }

        private void SendInfoIfIsAdminClient(int clientID)
        {
            if (_clientRepository.GetClientById(clientID).IsAdmin) {
                ViewData["admin"] = 1;
            }
            else {
                ViewData["admin"] = 0;
            }
        }

    }
}
