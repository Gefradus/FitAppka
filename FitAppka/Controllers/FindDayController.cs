using FitAppka.Models;
using FitAppka.Repository;
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
        private readonly IMealRepository _mealRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRespository;
        
        public FindDayController(IClientRepository clientRepository, IDayRepository dayRepository, IMealRepository mealRepository, IProductRepository productRepository)
        {
            _productRespository = productRepository;
            _mealRepository = mealRepository;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
        }

        public IActionResult FindDay(int productID, int from, int to, int searchType)
        {
            var clientID = _clientRepository.GetClientByLogin(User.Identity.Name).ClientId;
            var listOfDays = FindDays(from, to, productID, searchType, clientID);

            CreateProductsList(productID);

            ViewData["searchType"] = searchType;
            ViewData["from"] = from;
            ViewData["to"] = to;  
            ViewData["daysID"] = listOfDays;
            ViewData["wheterFound"] = WheterDayFound(listOfDays);
            ViewData["clientID"] = clientID;
            ViewData["listByWater"] = WaterInDays(listOfDays);                        
            ViewData["listByKcal"] = CaloriesInDays(listOfDays);                   
            ViewData["productID"] = productID;                                  
            ViewData["listByGrammages"] = GrammageInDays(listOfDays, productID);  

            return View(_dayRepository.GetAllDays());
        }

        private void CreateProductsList(int productID)
        {
            List<SelectListItem> productsList = new List<SelectListItem>();
            foreach (var item in _productRespository.GetAllProducts())
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
                foreach (var day in _dayRepository.GetAllDays())
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
                foreach (var day in _dayRepository.GetAllDays())
                {
                    if (dayID == day.DayId)
                    { 
                        foreach (var meal in _mealRepository.GetAllMeals())
                        {
                            if (meal.DayId == dayID)
                            {          
                                if (meal.ProductId == productID)
                                {
                                    sumOfGrammages += meal.Grammage;
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
                foreach (var day in _dayRepository.GetAllDays())
                {
                    if (dayID == day.DayId)
                    {
                        foreach (var meal in _mealRepository.GetAllMeals())
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


        private List<int> FindDays(int from, int to, int productID, int searchType, int clientID){
            if (searchType == 1)
            {
                return FindDayByProduct(from, to, productID, clientID);
            }
            if (searchType == 2)
            {
                return FindDaysByCalories(from, to, clientID);
            }
            if (searchType == 3)
            {
                return FindDaysByWater(from, to, clientID);
            }
            
            return new List<int>();  
        }


        private List<int> FindDaysByWater(int from, int to, int clientID)
        {
            List<int> daysList = new List<int>();

            foreach (var item in _dayRepository.GetClientDays(clientID))
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
            List<int> listOfCaloriesInDays = new List<int>();
            List<int> listOfDaysID = new List<int>();
            List<int> listOfDays = new List<int>();
            var listOfMeals = _mealRepository.GetAllMeals();

            foreach (var item in _dayRepository.GetClientDays(clientID))
            {
                double? whichDayMeals = 0;
                foreach (var meal in listOfMeals)
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
            List<int> daysList = new List<int>();
            var mealsWithProduct = _mealRepository.GetAllMeals().Where(p => p.ProductId == productID);
            foreach (var item in _dayRepository.GetClientDays(clientID))
            { 
                int sumOfGrammages = 0;
                foreach (var meal in mealsWithProduct.Where(p => p.DayId == item.DayId))
                {
                    sumOfGrammages += meal.Grammage;
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