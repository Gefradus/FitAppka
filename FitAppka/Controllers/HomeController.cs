using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitAppka.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using FitAppka.Repository;

namespace NowyDotnecik.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly FitAppContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDayRepository _dayRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(FitAppContext context, IWebHostEnvironment env, IMealRepository mealRepository, IDayRepository dayRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _mealRepository = mealRepository;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Home(DateTime daySelected)
        {
            int clientID = GetLoggedInClientID();
            
            CheckIfAdmin(clientID);
            AddDayIfNotExists(daySelected, clientID);
            int daySelectedID = _dayRepository.GetClientDayByDate(daySelected, clientID).DayId;

            ViewData["day"] = daySelected;
            ViewData["date"] = DateFormat(daySelected);
            ViewData["datepick"] = daySelected.ToString("yyyy-MM-dd");
            ViewData["path"] = _env.WebRootPath.ToString();
            SendDataToView(clientID, daySelectedID, 0, 0);
            
            CheckMealsOfTheDay(daySelected, clientID);
            CountCalories(1, "breakfastKcal", daySelected, clientID);
            CountCalories(2, "lunchKcal", daySelected, clientID);
            CountCalories(3, "dinnerKcal", daySelected, clientID);
            CountCalories(4, "dessertKcal", daySelected, clientID);
            CountCalories(5, "snackKcal", daySelected, clientID);
            CountCalories(6, "supperKcal", daySelected, clientID);
            ShowWater(daySelected, clientID);
            CountProgress(daySelected, clientID);

            var fitAppContext = _context.Meal.Include(m => m.Day).Include(p => p.Product).Where(m => m.DayId == daySelectedID);

            return View(await fitAppContext.ToListAsync());
            
        }

        private void CheckIfAdmin(int clientID)
        {
            Client client = _clientRepository.GetClient(clientID);
            if (client.IsAdmin)
            {
                ViewData["admin"] = 1;
            }
            else
            {
                ViewData["admin"] = 0;
            }
        }

        private int GetLoggedInClientID()
        {
            return _context.Client.Where(c => c.Login.ToLower().Equals(User.Identity.Name.ToLower())).Select(c => c.ClientId).FirstOrDefault();
        }

        public IActionResult Start()
        {
            if (IsItFirstLaunch(GetLoggedInClientID()))
            {
                return RedirectToAction("Settings", "Settings", new { czyPierwsze = 1 });
            }
            
            return RedirectToAction(nameof(Home), new { daySelected = DateTime.Now.Date});
        }

        private bool IsItFirstLaunch(int clientID)
        {
            return _clientRepository.GetClient(clientID).CarbsTarget == null;
        }

        public IActionResult Return(int clientID, int dayID)
        {
            DateTime daySelected;
            DateTime day = _dayRepository.GetDayDateTime(dayID);
            if (day != null && dayID != 0 && clientID != 0)
            {
                daySelected = day;
            }
            else
            {
                daySelected = DateTime.Now.Date;
                clientID = GetLoggedInClientID();
            }
  
            return RedirectToAction(nameof(Home), new { clientID, daySelected});
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

        private void SendDataToView(int clientID, int dayID, int inWhich, int? mealID)
        {
            ViewData["clientID"] = clientID;
            ViewData["dayID"] = dayID;
            ViewData["inWhich"] = inWhich;
            ViewData["mealID"] = mealID;
        }


        private string DateFormat(DateTime daySelected)
        {
            int dayOfWeek = (int) daySelected.DayOfWeek;
            string day = "";
            string month = "";

            if(dayOfWeek == 0){ day = "Niedziela, "; }
            if(dayOfWeek == 1){ day = "Poniedziałek, "; }
            if(dayOfWeek == 2){ day = "Wtorek, "; }
            if(dayOfWeek == 3){ day = "Środa, "; }
            if(dayOfWeek == 4){ day = "Czwartek, "; }
            if(dayOfWeek == 5){ day = "Piątek, "; }   
            if(dayOfWeek == 6){ day = "Sobota, "; }

            if (daySelected.Month == 1) { month = "sty, "; }
            if (daySelected.Month == 2) { month = "lut, "; }
            if (daySelected.Month == 3) { month = "mar, "; }
            if (daySelected.Month == 4) { month = "kwi, "; }
            if (daySelected.Month == 5) { month = "maj, "; }
            if (daySelected.Month == 6) { month = "czer, "; }
            if (daySelected.Month == 7) { month = "lip, "; }
            if (daySelected.Month == 8) { month = "sie, "; }
            if (daySelected.Month == 9) { month = "wrz, "; }
            if (daySelected.Month == 10) { month = "paź, "; }
            if (daySelected.Month == 11) { month = "lis, "; }
            if (daySelected.Month == 12) { month = "gru, "; }
            
            if (daySelected == DateTime.Now.Date)
            {
                day = "Dzisiaj, ";
            }

            if (daySelected == DateTime.Now.Date.AddDays(-1))
            {
                day = "Wczoraj, ";
            }

            if (daySelected == DateTime.Now.Date.AddDays(1))
            {
                day = "Jutro, ";
            }

            return day + daySelected.Day + " " + month + daySelected.Year;
        }


        private void AddDayIfNotExists(DateTime daySelected, int clientID)
        {
            int count = _context.Day.Count(dz => dz.Date == daySelected && dz.ClientId == clientID);
            if (count == 0)
            {
                var client = _clientRepository.GetClient(clientID);

                _dayRepository.Add(new Day()
                {
                    Date = daySelected,
                    ClientId = clientID,
                    Breakfast = client.Breakfast,
                    Lunch = client.Lunch,
                    Dinner = client.Dinner,
                    Dessert = client.Dessert,
                    Snack = client.Snack,
                    Supper = client.Supper,
                    ProteinTarget = client.ProteinTarget,
                    FatTarget = client.FatTarget,
                    CarbsTarget = client.CarbsTarget,
                    CalorieTarget = client.CalorieGoal,
                    WaterDrunk = 0,
                }); 
 
            }

        }

        [HttpGet]
        public IActionResult ChangeDay(string day) 
        { 
            return RedirectToAction(nameof(Home), new { dzienWybrany = Convert.ToDateTime(day) });
        }


        private void CountCalories(int whichMeal, string kcalMeal, DateTime daySelected, int clientID)
        {
            AddDayIfNotExists(daySelected, clientID);
            var meal = _context.Meal.Where(m => m.InWhichMealOfTheDay == whichMeal && m.Day.ClientId == clientID && m.Day.Date == daySelected);
            List<double> listOfCalories = meal.Select(m => m.Calories).ToList();
            double? kcal = 0;

            foreach(var item in listOfCalories)
            { kcal += item;}
            
            ViewData[kcalMeal] = Math.Round((decimal)kcal, 0, MidpointRounding.AwayFromZero);
        }
  
        private void CheckMealsOfTheDay(DateTime daySelected, int clientID)
        {
            AddDayIfNotExists(daySelected, clientID);
            Day day = _dayRepository.GetClientDayByDate(daySelected, clientID);

            ViewData["breakfast"] = day.Breakfast;
            ViewData["lunch"] = day.Lunch;
            ViewData["dinner"] = day.Dinner;
            ViewData["dessert"] = day.Dessert;
            ViewData["snack"] = day.Snack;
            ViewData["supper"] = day.Supper;
        }

        private void CountProgress(DateTime daySelected, int clientID)
        {
            AddDayIfNotExists(daySelected, clientID);

            var meals = _context.Meal.Where(m => m.Day.ClientId == clientID && m.Day.Date == daySelected);
            double kcal = SumAllListItems(meals.Select(m => m.Calories).ToList());
            double proteins = SumAllListItems(meals.Select(m => m.Proteins).ToList());
            double fats = SumAllListItems(meals.Select(m => m.Fats).ToList());
            double carbs = SumAllListItems(meals.Select(m => m.Carbohydrates).ToList());

            var day = _dayRepository.GetClientDayByDate(daySelected, clientID);
            double? calorieTarget = day.CalorieTarget;
            double? proteinTarget = day.ProteinTarget;
            double? fatTarget = day.FatTarget;
            double? carbsTarget = day.CarbsTarget;

            ViewData["calories"] = kcal;
            ViewData["proteins"] = proteins;
            ViewData["carbs"] = carbs;
            ViewData["fats"] = fats;
            ViewData["calories0"] = Math.Round((decimal)kcal, 0, MidpointRounding.AwayFromZero);
            ViewData["proteins0"] = Math.Round((decimal)proteins, 0, MidpointRounding.AwayFromZero);
            ViewData["carbs0"] = Math.Round((decimal)carbs, 0, MidpointRounding.AwayFromZero);
            ViewData["fats0"] = Math.Round((decimal)fats, 0, MidpointRounding.AwayFromZero);
            ViewData["calorieTarget"] = calorieTarget;
            ViewData["proteinTarget"] = proteinTarget;
            ViewData["fatTarget"] = fatTarget;
            ViewData["carbsTarget"] = carbsTarget;
            ViewData["%calories"] = (int)(kcal / calorieTarget * 100);
            ViewData["%carbs"] = (int)(carbs / carbsTarget * 100);
            ViewData["%fats"] = (int)(fats / fatTarget * 100);
            ViewData["%proteins"] = (int)(proteins / proteinTarget * 100);
        }


        private double SumAllListItems(List<double> lista)
        {
            double dana = 0;
            foreach (var item in lista) { dana += item; }
            return dana;
        }

        private void ShowWater(DateTime daySelected, int clientID)
        {
            ViewData["water"] = _dayRepository.GetClientDayByDate(daySelected, clientID).WaterDrunk;
        }

        public IActionResult AddWater(int dayID, int clientID)
        {
            Day day = _dayRepository.GetDay(dayID);

            try 
            {
                int addedWater = int.Parse(HttpContext.Request.Form["AddedWater"]);
                day.WaterDrunk += addedWater;
                _dayRepository.Update(day);
            }
            catch {}
            return RedirectToAction(nameof(Home), new { clientID, daySelected = day.Date });
        }


        public IActionResult EditWater(int dayID, int clientID)
        {
            Day day = _dayRepository.GetDay(dayID);
            try 
            {
                int editedWater = int.Parse(HttpContext.Request.Form["EditedWater"]);
                day.WaterDrunk = editedWater;
                _dayRepository.Update(day);
            }
            catch {}
            return RedirectToAction(nameof(Home), new { clientID, daySelected = day.Date });
        }

     
        private void SetTheMeal(Meal meal, DateTime daySelected, int clientID)
        {
            var product = _productRepository.GetProduct(meal.ProductId);

            double? kcalIn100gr = product.Calories;
            double? proteinsIn100gr = product.Proteins;
            double? carbsIn100gr = product.Carbohydrates;
            double? fatsIn100gr = product.Fats;

            decimal calories = (decimal)(meal.Grammage * kcalIn100gr / 100);
            decimal proteins = (decimal)(meal.Grammage * proteinsIn100gr / 100);
            decimal carbs = (decimal)(meal.Grammage * carbsIn100gr / 100);
            decimal fats = (decimal)(meal.Grammage * fatsIn100gr / 100);

            meal.Calories = (double) Math.Round(calories, 1, MidpointRounding.AwayFromZero);
            meal.Proteins = (double) Math.Round(proteins, 1, MidpointRounding.AwayFromZero);
            meal.Carbohydrates = (double) Math.Round(carbs, 1, MidpointRounding.AwayFromZero);
            meal.Fats = (double) Math.Round(fats, 1, MidpointRounding.AwayFromZero);

            AddDayIfNotExists(daySelected, clientID);
            meal.DayId = _dayRepository.GetClientDayByDate(daySelected, clientID).DayId;
        }

        public IActionResult Add(int clientID, int dayID, int inWhich)
        {
            SendDataToView(clientID, dayID, inWhich, 0);
            return View();
        }


        [HttpPost]
        public JsonResult Add(int inWhich, int dayID, int clientID, int grammage, int productID)
        {
            DateTime daySelected = _dayRepository.GetDayDateTime(dayID);
            Meal meal = new Meal()
            {
                Grammage = grammage,
                ProductId = productID,
                InWhichMealOfTheDay = inWhich,
                DayId = dayID,
            };

            SetTheMeal(meal, daySelected, clientID);
            _mealRepository.Add(meal);
            return Json(true);
        }


        [HttpPost]
        public IActionResult Edit(int mealID, int grammage)
        {
            var meal = _mealRepository.GetMeal(mealID);
            var day = _dayRepository.GetDay(meal.DayId);

            try
            { 
                if (meal == null) { return NotFound(); }
                meal.Grammage = grammage;
                SetTheMeal(meal, (DateTime)day.Date, day.ClientId);
                _mealRepository.Update(meal);
            }
            catch (DbUpdateConcurrencyException) { }
            return Json(true);
        }

        [HttpPost]
        public IActionResult Delete(int mealID)
        {
            _mealRepository.Delete(mealID);
            return Json(false);
        }

    }
}
