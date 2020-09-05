using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServicesImpl
{
    public class HomePageServiceImpl
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMealRepository mealRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IProductRepository _productRepository;
        private readonly FitAppContext _context;

        public HomePageServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository, 
            IMealRepository mealRepository, IProductRepository productRepository, FitAppContext context)
        {
            _context = context;
            _productRepository = productRepository;
            _mealRepository = mealRepository;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
        }

        public void Home(DateTime daySelected)
        {
            int clientID = _clientRepository.GetLoggedInClient().ClientId;
            AddDayIfNotExists(daySelected, clientID);
            SendInfoAboutMealsToView(daySelected, clientID);
        }

        public string DateFormat(DateTime daySelected)
        {
            int dayOfWeek = (int)daySelected.DayOfWeek;
            string day = "";
            string month = "";

            if (dayOfWeek == 0) { day = "Niedziela, "; }
            if (dayOfWeek == 1) { day = "Poniedziałek, "; }
            if (dayOfWeek == 2) { day = "Wtorek, "; }
            if (dayOfWeek == 3) { day = "Środa, "; }
            if (dayOfWeek == 4) { day = "Czwartek, "; }
            if (dayOfWeek == 5) { day = "Piątek, "; }
            if (dayOfWeek == 6) { day = "Sobota, "; }

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

            if (daySelected == DateTime.Now.Date) { day = "Dzisiaj, "; }
            if (daySelected == DateTime.Now.Date.AddDays(-1)) { day = "Wczoraj, "; }
            if (daySelected == DateTime.Now.Date.AddDays(1)) { day = "Jutro, "; }

            return day + daySelected.Day + " " + month + daySelected.Year;
        }


        public bool IsItFirstLaunch()
        {
            return _clientRepository.GetLoggedInClient().CarbsTarget == null;
        }

        private IQueryable<Meal> GetMealsOfTheDay(DateTime dayDate)
        {
            return _context.Meal.Where(m => m.Day.ClientId == _clientRepository.GetLoggedInClientId() && m.Day.Date == dayDate);
        }


        public double SumAllKcalInDay(DateTime daySelected)
        {
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Calories).ToList());
        }

        public double SumAllProteinsInDay(DateTime daySelected)
        {
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Proteins).ToList());
        }

        public double SumAllCarbsInDay(DateTime daySelected)
        {
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Calories).ToList());
        }

        public double SumAllFatsInDay(DateTime daySelected)
        {
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Fats).ToList());
        }

        public decimal Round(double var)
        {
            return Math.Round((decimal)var, 0, MidpointRounding.AwayFromZero);
        }

        public int CountPercentageOfTarget(double var, int? target)
        {
            return (int)(var / target * 100);
        }

        

        private void AddDayIfNotExists(DateTime daySelected, int clientID)
        {
            if (_context.Day.Count(dz => dz.Date == daySelected && dz.ClientId == clientID) == 0)
            {
                var client = _clientRepository.GetClientById(clientID);

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

        private void SendInfoAboutMealsToView(DateTime daySelected, int clientID)
        {
            CountCalories(1, "breakfastKcal", daySelected, clientID);
            CountCalories(2, "lunchKcal", daySelected, clientID);
            CountCalories(3, "dinnerKcal", daySelected, clientID);
            CountCalories(4, "dessertKcal", daySelected, clientID);
            CountCalories(5, "snackKcal", daySelected, clientID);
            CountCalories(6, "supperKcal", daySelected, clientID);
        }

        private double SumAllListItems(List<double> lista)
        {
            double dana = 0;
            foreach (var item in lista) { dana += item; }
            return dana;
        }

        public void AddWater(int dayID, string form)
        {
            Day day = _dayRepository.GetDay(dayID);
            if (day.ClientId == _clientRepository.GetLoggedInClientId())
            {
                try
                {
                    int addedWater = int.Parse(form);
                    day.WaterDrunk += addedWater;
                    _dayRepository.Update(day);
                }
                catch { }
            }
        }

        public void EditWater(int dayID, string form)
        {
            Day day = _dayRepository.GetDay(dayID);
            if (day.ClientId == _clientRepository.GetLoggedInClientId())
            {
                try
                {
                    int editedWater = int.Parse(form);
                    day.WaterDrunk = editedWater;
                    _dayRepository.Update(day);
                }
                catch { }
            }
        }


        public void EditMeal(int mealID, int grammage)
        {
            var meal = _mealRepository.GetMeal(mealID);
            var day = _dayRepository.GetDay(meal.DayId);

            if (day.ClientId == _clientRepository.GetLoggedInClientId())
            {
                try
                {
                    meal.Grammage = grammage;
                    SetTheMeal(meal, (DateTime)day.Date);
                    _mealRepository.Update(meal);
                }
                catch {}
            }
        }

        public void DeleteMeal(int mealID)
        {
            if (_dayRepository.GetDay(_mealRepository.GetMeal(mealID).DayId).ClientId == _clientRepository.GetLoggedInClientId())
            {
                _mealRepository.Delete(mealID);
            }
        }

        public void AddMeal(int inWhich, int dayID, int grammage, int productID)
        {
            if (_dayRepository.GetDay(dayID).ClientId == _clientRepository.GetLoggedInClientId())
            {
                Meal meal = new Meal()
                {
                    Grammage = grammage,
                    ProductId = productID,
                    InWhichMealOfTheDay = inWhich,
                    DayId = dayID,
                };

                SetTheMeal(meal, _dayRepository.GetDayDateTime(dayID));
                _mealRepository.Add(meal);
            }
        }


        private void CountCalories(int whichMeal, string kcalMeal, DateTime daySelected, int clientID)
        {
            AddDayIfNotExists(daySelected, clientID);
            var meal = _context.Meal.Where(m => m.InWhichMealOfTheDay == whichMeal && m.Day.ClientId == clientID && m.Day.Date == daySelected);
            List<double> listOfCalories = meal.Select(m => m.Calories).ToList();
            double? kcal = 0;

            foreach (var item in listOfCalories)
            { kcal += item; }

            ViewData[kcalMeal] = Math.Round((decimal)kcal, 0, MidpointRounding.AwayFromZero);
        }

        private void SetTheMeal(Meal meal, DateTime daySelected)
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

            meal.Calories = (double)Math.Round(calories, 1, MidpointRounding.AwayFromZero);
            meal.Proteins = (double)Math.Round(proteins, 1, MidpointRounding.AwayFromZero);
            meal.Carbohydrates = (double)Math.Round(carbs, 1, MidpointRounding.AwayFromZero);
            meal.Fats = (double)Math.Round(fats, 1, MidpointRounding.AwayFromZero);

            AddDayIfNotExists(daySelected, _clientRepository.GetLoggedInClientId());
            meal.DayId = _dayRepository.GetClientDayByDate(daySelected, _clientRepository.GetLoggedInClientId()).DayId;
        }


    }
}
