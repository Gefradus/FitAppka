using FitAppka.Models;
using FitAppka.Repository;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServicesImpl
{
    public class HomePageServiceImpl : IHomePageService
    {
        private readonly IClientRepository _clientRepository;
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
            AddDayIfNotExists(daySelected, _clientRepository.GetLoggedInClient().ClientId);
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


        public bool IsItFirstLaunch() {
            Client client = _clientRepository.GetLoggedInClient();
            return client.CarbsTarget == null || client.ProteinTarget == null || client.FatTarget == null || client.CalorieGoal == null;
        }

        private IQueryable<Meal> GetMealsOfTheDay(DateTime dayDate){
            return _context.Meal.Where(m => m.Day.ClientId == _clientRepository.GetLoggedInClientId() && m.Day.Date == dayDate);
        }

        public double SumAllKcalInDay(DateTime daySelected){
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Calories).ToList());
        }

        public double SumAllProteinsInDay(DateTime daySelected){
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Proteins).ToList());
        }

        public double SumAllCarbsInDay(DateTime daySelected){
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Calories).ToList());
        }

        public double SumAllFatsInDay(DateTime daySelected){
            return SumAllListItems(GetMealsOfTheDay(daySelected).Select(m => m.Fats).ToList());
        }

        public int CountPercentageOfTarget(double var, int? target){
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

        public void AddWater(int dayID, StringValues form)
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

        public void EditWater(int dayID, StringValues form)
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


        public void EditMeal(int id, int grammage)
        {
            var meal = _mealRepository.GetMeal(id);
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

        public void DeleteMeal(int id)
        {
            if (_dayRepository.GetDay(_mealRepository.GetMeal(id).DayId).ClientId == _clientRepository.GetLoggedInClientId())
            {
                _mealRepository.Delete(id);
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

        public decimal CountCalories(int whichMeal, DateTime daySelected, int clientID)
        {
            AddDayIfNotExists(daySelected, clientID);
            var meals = _context.Meal.Where(m => m.InWhichMealOfTheDay == whichMeal && m.Day.ClientId == clientID && m.Day.Date == daySelected);
            return Round((double) SumAllListItems(meals.Select(m => m.Calories).ToList()));
        }

        private void SetTheMeal(Meal meal, DateTime daySelected)
        {
            Product product = _productRepository.GetProduct(meal.ProductId);

            meal.Calories = RoundDouble(meal.Grammage * product.Calories / 100);
            meal.Proteins = RoundDouble(meal.Grammage * product.Proteins / 100);
            meal.Carbohydrates = RoundDouble(meal.Grammage * product.Carbohydrates / 100);
            meal.Fats = RoundDouble(meal.Grammage * product.Fats / 100);

            AddDayIfNotExists(daySelected, _clientRepository.GetLoggedInClientId());
            meal.DayId = _dayRepository.GetClientDayByDate(daySelected, _clientRepository.GetLoggedInClientId()).DayId;
        }

        public decimal Round(double number)
        {
            return Math.Round((decimal)number, 0, MidpointRounding.AwayFromZero);
        }

        private double RoundDouble(double? number)
        {
            return (double)Math.Round((decimal)number, 1, MidpointRounding.AwayFromZero);
        }

        public double SumAllListItems(List<double> list)
        {
            double var = 0;
            foreach (var item in list) { var += item; }
            return var;
        }
    }
}
