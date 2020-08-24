using FitAppka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class FindDayController : Controller
    {
        private readonly FitAppContext _context;
        public FindDayController(FitAppContext context)
        {
            _context = context;
        }

        public IActionResult FindDay(int productID, int from, int to, int searchType)
        {
            var clientID = _context.Client.Where(k => k.Login.ToLower() == User.Identity.Name.ToLower()).Select(c => c.ClientId).FirstOrDefault();
            
            var listOfDays = FindDays(from, to, productID, searchType, clientID);

            CreateProductsList(productID);

            ViewData["searchType"] = searchType;
            ViewData["from"] = to;
            ViewData["to"] = from;  
            ViewData["daysID"] = listOfDays;
            ViewData["wheterFound"] = WheterDayFound(listOfDays);
            ViewData["clientID"] = clientID;

            ViewData["listByWater"] = WaterInDays(listOfDays);                        //gdy wyszukujemy po wodzie
            ViewData["listByKcal"] = CaloriesInDays(listOfDays);                   //gdy wyszukujemy po kaloriach
            ViewData["productID"] = productID;                                  //gdy wyszukujemy po produktach
            ViewData["listByGrammages"] = GrammageInDays(listOfDays, productID);  //gdy wyszukujemy po produktach

            return View(_context.Day.ToList());
        }

        private void CreateProductsList(int productID)
        {
            List<SelectListItem> productsList = new List<SelectListItem>();
            foreach (var item in _context.Product)
            {
                if (item.ProductId == productID)
                {
                    productsList.Add(new SelectListItem() { Text = item.ProductName + ", " + (int)item.Calories + "kcal", Value = item.ProductId + "", Selected = true });
                }
                else
                {
                    productsList.Add(new SelectListItem() { Text = item.ProductName + ", " + (int)item.Calories + "kcal", Value = item.ProductId + "" });
                }
            }

            ViewData["allProducts"] = productsList; 
        }


        private List<int?> WaterInDays(List<int> daysList)
        {
            List<int?> waterList = new List<int?>();

            foreach (var dayID in daysList)
            {
                int? sumOfDrunkWater = 0;
                foreach (var day in _context.Day.ToList())
                {
                    if (dayID == day.DayId)
                    {
                        sumOfDrunkWater += day.WaterDrunk;
                    }
                }
                waterList.Add(sumOfDrunkWater);             
            }

            return waterList;
        }

        private List<int> GrammageInDays(List<int> daysList, int productID)
        {
            List<int> grammagesList = new List<int>();

            foreach (var dayID in daysList)
            {
                int sumOfGrammages = 0;
                foreach (var day in _context.Day.ToList())
                {
                    if (dayID == day.DayId)
                    { 
                        foreach (var meal in _context.Meal.ToList())
                        {
                            if (meal.DayId == dayID)
                            {          
                                if (meal.ProductId == productID)
                                {
                                    sumOfGrammages += (int)meal.Grammage;
                                }
                            }
                        }
                    }
                }
                grammagesList.Add(sumOfGrammages);
            }
            return grammagesList;
        }

        private List<decimal> CaloriesInDays(List<int> daysList)
        {
            List<decimal> caloriesList = new List<decimal>();
            foreach (var dayID in daysList)
            {
                double? sumOfCalories = 0;
                foreach (var day in _context.Day.ToList())
                {
                    if (dayID == day.DayId)
                    {
                        foreach (var meal in _context.Meal.ToList())
                        {
                            if (meal.DayId == dayID)
                            {
                                sumOfCalories += meal.Calories;
                            }
                        }
                    }
                }
                caloriesList.Add(Math.Round((decimal)sumOfCalories, 0, MidpointRounding.AwayFromZero));
            }
            return caloriesList;
        }

        private bool WheterDayFound(List<int> listaDni)
        {
            return listaDni.Count > 0;
        }


        private List<int> FindDays(int od, int dO, int produktID, int typSzukania, int klientID){
            if (typSzukania == 1)
            {
                return FindDayByProduct(od, dO, produktID, klientID);
            }
            if (typSzukania == 2)
            {
                return FindDaysByCalories(od, dO, klientID);
            }
            if (typSzukania == 3)
            {
                return FindDaysByWater(od, dO, klientID);
            }
            
            return new List<int>();  
        }


        private List<int> FindDaysByWater(int from, int to, int clientID)
        {
            List<int> daysList = new List<int>();

            foreach (var item in _context.Day.Where(d => d.ClientId == clientID).ToList())
            {
                if(item.WaterDrunk >= from && (item.WaterDrunk <= to || to == 0))
                {
                    daysList.Add(item.DayId);
                }
            }

            return daysList;
        }


        private List<int> FindDaysByCalories(int from, int to, int clientID)
        {
            List<Meal> meals = _context.Meal.ToList(); 
            List<int> listOfCaloriesInDays = new List<int>();
            List<int> listOfDaysID = new List<int>();
            List<int> listOfDays = new List<int>();

            foreach (var item in _context.Day.Where(d => d.ClientId == clientID).ToList())
            {
                double? whichDayMeals = 0;
                foreach (var meal in meals)
                {
                    if (meal.DayId == item.DayId)
                    {
                        whichDayMeals += meal.Calories;
                    }
                }
                listOfCaloriesInDays.Add((int)whichDayMeals);
                listOfDaysID.Add(item.DayId);
            }

            int whichDay = 0;
            foreach(var calories in listOfCaloriesInDays)
            {
                if (calories >= from && (calories <= to || to == 0))
                {
                    listOfDays.Add(listOfDaysID[whichDay]);
                }

                whichDay++;
            }

            return listOfDays;
        }

        private List<int> FindDayByProduct(int from, int to, int productID, int clientID)
        {
            List<Meal> meals = _context.Meal.Where(p => p.ProductId == productID).ToList();
            List<int> daysList = new List<int>();

            foreach(var item in _context.Day.Where(d => d.ClientId == clientID).ToList())
            {
                List<Meal> mealsList = meals.Where(p => p.DayId == item.DayId).ToList();
                
                int sumOfGrammages = 0;
                foreach (var meal in mealsList)
                {
                    sumOfGrammages += (int)meal.Grammage;
                }

                if (sumOfGrammages >= from && (sumOfGrammages <= to || to == 0) && sumOfGrammages != 0)
                {
                    daysList.Add(item.DayId);
                }
            }

            return daysList;
        }
    }
}