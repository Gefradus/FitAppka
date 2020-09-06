using FitAppka.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class FindDayServiceImpl : IFindDayService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IProductRepository _productRepository;

        public FindDayServiceImpl(IMealRepository mealRepository, IDayRepository dayRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _dayRepository = dayRepository;
            _mealRepository = mealRepository;
        }


        public List<int> FindDays(int from, int to, int productID, int searchType, int clientID)
        {
            if (searchType == 1) {
                return FindDayByProduct(from, to, productID, clientID);
            }
            if (searchType == 2) {
                return FindDaysByCalories(from, to, clientID);
            }
            if (searchType == 3) {
                return FindDaysByWater(from, to, clientID);
            }

            return new List<int>();
        }



        private List<int> FindDayByProduct(int from, int to, int productID, int clientID)
        {
            var daysList = new List<int>();
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

        private List<int> FindDaysByCalories(int from, int to, int clientID)
        {
            var listOfCaloriesInDays = new List<int>();
            var listOfDaysID = new List<int>();
            var listOfDays = new List<int>();
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
            foreach (var calories in listOfCaloriesInDays)
            {
                if (calories >= from && (calories <= to || to == 0))
                {
                    listOfDays.Add(listOfDaysID[whichDay]);
                }

                whichDay++;
            }

            return listOfDays;
        }

        private List<int> FindDaysByWater(int from, int to, int clientID)
        {
            var daysList = new List<int>();

            foreach (var item in _dayRepository.GetClientDays(clientID))
            {
                if (item.WaterDrunk >= from && (item.WaterDrunk <= to || to == 0))
                {
                    daysList.Add(item.DayId);
                }
            }

            return daysList;
        }


        public List<SelectListItem> CreateProductsList(int productID)
        {
            var productsList = new List<SelectListItem>();
            foreach (var item in _productRepository.GetAllProducts())
            {
                SelectListItem selectListItem = new SelectListItem() { 
                    Text = item.ProductName + ", " + (int)item.Calories + "kcal", 
                    Value = item.ProductId + "" 
                };

                if (item.ProductId == productID) {
                    selectListItem.Selected = true;
                }
                
                productsList.Add(selectListItem);
            }

            return productsList;
        }


        public List<int?> WaterInDays(List<int> daysList)
        {
            var waterList = new List<int?>();

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

        public List<int> GrammageInDays(List<int> daysList, int productID)
        {
            var grammagesList = new List<int>();

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

        public List<decimal> CaloriesInDays(List<int> daysList)
        {
            var caloriesList = new List<decimal>();
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

        public bool WheterDayFound(List<int> listaDni)
        {
            return listaDni.Count > 0;
        }


    }
}
