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

        public async Task<IActionResult> Home(DateTime daySelected)
        {
            _homeService.Home(daySelected);
            SendDataToView(daySelected);
            return View(await _context.Meal.Include(m => m.Day).Include(p => p.Product).
                Where(m => m.DayId == _dayRepository.GetClientDayByDate(daySelected, _clientRepository.GetLoggedInClientId()).DayId).ToListAsync());
        }

        private void SendDataToView(DateTime daySelected)
        {
            int clientID = _clientRepository.GetLoggedInClientId();
            Day day = _dayRepository.GetClientDayByDate(daySelected, clientID);
            SendBasicData(daySelected, clientID, day);
            CheckIfAdmin(clientID);
            CountProgress(day);
            CheckMealsOfTheDay(day);
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

        private void CountProgress(Day day)
        {
            var daySelected = (DateTime) day.Date;
            var meals = _context.Meal.Where(m => m.Day == day);
            
            ViewData["calories"] = _homeService.SumAllKcalInDay(daySelected);
            ViewData["proteins"] = _homeService.SumAllProteinsInDay(daySelected);
            ViewData["carbs"] = _homeService.SumAllCarbsInDay(daySelected);
            ViewData["fats"] = _homeService.SumAllFatsInDay(daySelected);
            ViewData["calories0"] = _homeService.Round(_homeService.SumAllKcalInDay(daySelected));
            ViewData["proteins0"] = _homeService.Round(_homeService.SumAllProteinsInDay(daySelected));
            ViewData["carbs0"] = _homeService.Round(_homeService.SumAllCarbsInDay(daySelected));
            ViewData["fats0"] = _homeService.Round(_homeService.SumAllFatsInDay(daySelected));
            ViewData["calorieTarget"] = day.CalorieTarget;
            ViewData["proteinTarget"] = day.ProteinTarget;
            ViewData["fatTarget"] = day.FatTarget;
            ViewData["carbsTarget"] = day.CarbsTarget;
            ViewData["%calories"] = _homeService.CountPercentageOfTarget(_homeService.SumAllKcalInDay(daySelected), day.CalorieTarget);
            ViewData["%carbs"] = _homeService.CountPercentageOfTarget(_homeService.SumAllCarbsInDay(daySelected), day.CarbsTarget);
            ViewData["%fats"] = _homeService.CountPercentageOfTarget(_homeService.SumAllFatsInDay(daySelected), day.FatTarget);
            ViewData["%proteins"] = _homeService.CountPercentageOfTarget(_homeService.SumAllProteinsInDay(daySelected), day.ProteinTarget);
        }

        private void CheckMealsOfTheDay(Day day)
        {
            ViewData["breakfast"] = day.Breakfast;
            ViewData["lunch"] = day.Lunch;
            ViewData["dinner"] = day.Dinner;
            ViewData["dessert"] = day.Dessert;
            ViewData["snack"] = day.Snack;
            ViewData["supper"] = day.Supper;
        }

        private void CheckIfAdmin(int clientID) 
        {
            if (_clientRepository.GetClientById(clientID).IsAdmin) {
                ViewData["admin"] = 1;
            }
            else {
                ViewData["admin"] = 0;
            }
        }

        [HttpGet]
        public IActionResult Start()
        {
            if (_homeService.IsItFirstLaunch())
            {
                return RedirectToAction("Settings", "Settings");
            }
            
            return RedirectToAction(nameof(Home), new { daySelected = DateTime.Now.Date});
        }

        [HttpGet]
        public IActionResult Return(int dayID)
        {
            DateTime daySelected;
            DateTime day = _dayRepository.GetDayDateTime(dayID);
            if (day != null && dayID != 0) { daySelected = day; }
            else { daySelected = DateTime.Now.Date; }
  
            return RedirectToAction(nameof(Home), new { clientID = _clientRepository.GetLoggedInClientId(), daySelected});
        }


        public IActionResult ChooseDay(int id, int dayID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);

            if (id == 1)
            {
                daySelected = daySelected.AddDays(-1);
                return RedirectToAction(nameof(Home), new { daySelected });
            }
            else
            {
                daySelected = daySelected.AddDays(1);
                return RedirectToAction(nameof(Home), new { daySelected });
            }
        }

        [HttpGet]
        public IActionResult ChangeDay(string day) 
        { 
            return RedirectToAction(nameof(Home), new { daySelected = Convert.ToDateTime(day) });
        }

        [HttpPost]
        public IActionResult AddWater(int dayID)
        {
            _homeService.AddWater(dayID, HttpContext.Request.Form["AddedWater"]);
            return RedirectToAction(nameof(Home), new { daySelected = _dayRepository.GetDayDateTime(dayID) });
        }

        [HttpPost]
        public IActionResult EditWater(int dayID)
        {
            _homeService.EditWater(dayID, HttpContext.Request.Form["EditedWater"]);
            return RedirectToAction(nameof(Home), new { daySelected = _dayRepository.GetDayDateTime(dayID) });
        }

        [HttpPost]
        public JsonResult Add(int inWhich, int dayID, int grammage, int productID)
        {
            _homeService.AddMeal(inWhich, dayID, grammage, productID);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(int mealID, int grammage)
        {
            _homeService.EditMeal(mealID, grammage);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(int mealID)
        {
            _homeService.DeleteMeal(mealID);
            return Json(false);
        }
    }
}
