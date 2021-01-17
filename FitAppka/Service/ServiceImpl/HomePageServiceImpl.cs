using FitnessApp.Models;
using FitnessApp.Models.DTO;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Service.ServiceImpl;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Service.ServicesImpl
{ 
    public class HomePageServiceImpl : IHomePageService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IDayManageService _dayManageService;
        private readonly IMealRepository _mealRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IDayManageService _dayService;
        private readonly IContentRootPathHandlerService _contentRootService;
        private readonly FitAppContext _context;

        public HomePageServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository, IDayManageService dayManageService, FitAppContext context,
            IMealRepository mealRepository, IProductRepository productRepository, IGoalsRepository goalsRepository, IContentRootPathHandlerService contentRootService)
        {
            _productRepository = productRepository;
            _mealRepository = mealRepository;
            _dayRepository = dayRepository;
            _dayManageService = dayManageService;
            _clientRepository = clientRepository;
            _goalsRepository = goalsRepository;
            _contentRootService = contentRootService;
            _dayService = dayManageService;
            _context = context;
        }

        public string DateFormat(DateTime daySelected)
        {
            string day = ((DayOfWeekEnum)daySelected.DayOfWeek).ToString();
            var today = DateTime.Now.Date;

            if (daySelected == today) { day = "Dzisiaj"; }
            if (daySelected == today.AddDays(-1)) { day = "Wczoraj"; }
            if (daySelected == today.AddDays(1)) { day = "Jutro"; }

            return day + ", " + daySelected.Day + " " + (MonthEnum)daySelected.Month + " " + daySelected.Year;
        }

        public bool IsItFirstLaunch() 
        {
            return _goalsRepository.GetClientGoals(_clientRepository.GetLoggedInClientId()) == null;
        }

        private List<Meal> GetMealsOfTheDay(DateTime dayDate) {
            return _mealRepository.GetAllMeals().Where(m => m.DayId == _dayManageService.GetDayIDByDate(dayDate)).ToList();
        }

        public double SumAllKcalInDay(DateTime daySelected){
            return GetMealsOfTheDay(daySelected).Select(m => m.Calories).ToList().Sum();
        }

        public double SumAllProteinsInDay(DateTime daySelected){
            return GetMealsOfTheDay(daySelected).Select(m => m.Proteins).ToList().Sum();
        }

        public double SumAllCarbsInDay(DateTime daySelected){
            return GetMealsOfTheDay(daySelected).Select(m => m.Carbohydrates).ToList().Sum();
        }

        public double SumAllFatsInDay(DateTime daySelected){
            return GetMealsOfTheDay(daySelected).Select(m => m.Fats).ToList().Sum();
        }

        public int CountPercentageOfTarget(double var, double target){
            return (int)(var / target * 100);
        }

        public void AddWater(int dayID, int addedWater)
        {
            Day day = _dayRepository.GetDay(dayID);
            if (day.ClientId == _clientRepository.GetLoggedInClientId())
            {
                try
                {
                    day.WaterDrunk += addedWater;
                    _dayRepository.Update(day);
                }
                catch { }
            }
        }

        public void EditWater(int dayID, int editedWater)
        {
            Day day = _dayRepository.GetDay(dayID);
            if (day.ClientId == _clientRepository.GetLoggedInClientId())
            {
                try
                {
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

        public DateTime AddMeal(int inWhich, int dayID, int grammage, int productID)
        {
            DateTime dateTime = _dayRepository.GetDayDateTime(dayID);
            if (_dayRepository.GetDay(dayID).ClientId == _clientRepository.GetLoggedInClientId())
            {
                Meal meal = new Meal()
                {
                    Grammage = grammage,
                    ProductId = productID,
                    InWhichMealOfTheDay = inWhich,
                    DayId = dayID,
                };

                SetTheMeal(meal, dateTime);
                _mealRepository.Add(meal);
            }
            return dateTime;
        }

        public decimal CountCalories(int whichMeal, DateTime daySelected)
        {
            _dayManageService.AddDayIfNotExists(daySelected);
            var meals = _mealRepository.GetMealsOfTheDay(_dayManageService.GetDayIDByDate(daySelected), whichMeal);
            return Round(meals.Select(m => m.Calories).ToList().Sum());
        }

        private void SetTheMeal(Meal meal, DateTime daySelected)
        {
            Product p = _productRepository.GetProduct(meal.ProductId);

            int gram = meal.Grammage;
            meal.Calories = RoundDouble(gram * p.Calories / 100);
            meal.Proteins = RoundDouble(gram * p.Proteins / 100);
            meal.Carbohydrates = RoundDouble(gram * p.Carbohydrates / 100);
            meal.Fats = RoundDouble(gram * p.Fats / 100);
            meal.Biotin = RoundDoubleMicro(gram, p.Biotin);
            meal.Calcium = RoundDoubleMicro(gram, p.Calcium);
            meal.Copper = RoundDoubleMicro(gram, p.Copper);
            meal.FolicAcid = RoundDoubleMicro(gram, p.FolicAcid);
            meal.Iodine = RoundDoubleMicro(gram, p.Iodine);
            meal.Iron = RoundDoubleMicro(gram, p.Iron);
            meal.Magnesium = RoundDoubleMicro(gram, p.Magnesium);
            meal.Phosphorus = RoundDoubleMicro(gram, p.Phosphorus);
            meal.Potassium = RoundDoubleMicro(gram, p.Potassium);
            meal.Selenium = RoundDoubleMicro(gram, p.Selenium);
            meal.Sodium = RoundDoubleMicro(gram, p.Sodium);
            meal.VitaminA = RoundDoubleMicro(gram, p.VitaminA);
            meal.VitaminB1 = RoundDoubleMicro(gram, p.VitaminB1);
            meal.VitaminB12 = RoundDoubleMicro(gram, p.VitaminB12);
            meal.VitaminB2 = RoundDoubleMicro(gram, p.VitaminB2);
            meal.VitaminB5 = RoundDoubleMicro(gram, p.VitaminB5);
            meal.VitaminB6 = RoundDoubleMicro(gram, p.VitaminB6);
            meal.VitaminC = RoundDoubleMicro(gram, p.VitaminC);
            meal.VitaminD = RoundDoubleMicro(gram, p.VitaminD);
            meal.VitaminE = RoundDoubleMicro(gram, p.VitaminE);
            meal.VitaminK = RoundDoubleMicro(gram, p.VitaminK);
            meal.VitaminPp = RoundDoubleMicro(gram, p.VitaminPp);
            meal.Zinc = RoundDoubleMicro(gram, p.Zinc);

            _dayManageService.AddDayIfNotExists(daySelected);
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

        private double RoundDoubleMicro(int gram, double? number)
        {
            return gram * (number == null ? 0 : (double)Math.Round((decimal)number, 4, MidpointRounding.AwayFromZero)) / 100;
        }

       
        public HomeDTO CreateHomeDTO(DateTime daySelected)
        {
            Day day = _dayService.GetLoggedInClientDayByDate(daySelected);

            return new HomeDTO()
            {
                Meals = _context.Meal.Include(m => m.Day).Include(p => p.Product).Where(m => m.DayId == _dayService.GetLoggedInClientDayByDate(daySelected).DayId).ToList(),
                BreakfastKcal = CountCalories(1, daySelected),
                LunchKcal = CountCalories(2, daySelected),
                DinnerKcal = CountCalories(3, daySelected),
                DessertKcal = CountCalories(4, daySelected),
                SnackKcal = CountCalories(5, daySelected),
                SupperKcal = CountCalories(6, daySelected),
                Day = daySelected,
                Date = DateFormat(daySelected),
                Datepick = daySelected.ToString("yyyy-MM-dd"),
                Path = _contentRootService.GetContentRootFileName(),
                ClientID = _clientRepository.GetLoggedInClientId(),
                DayID = day.DayId,
                Water = day.WaterDrunk,
                IsAdmin = _clientRepository.IsLoggedInClientAdmin() ? 1 : 0,
                Breakfast = (bool)day.Breakfast,
                Lunch = (bool)day.Lunch,
                Dinner = (bool)day.Dinner,
                Dessert = (bool)day.Dinner,
                Snack = (bool)day.Snack,
                Supper = (bool)day.Supper,
            };
        }
    }
}
