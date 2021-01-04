using FitAppka.Models.DTO;
using FitAppka.Models.DTO.DaySummaryDTO;
using FitAppka.Repository;
using FitAppka.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class ReportServiceImpl : IReportService
    {
        private readonly IDayRepository _dayRepository;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IHomePageService _homePageService;
        private readonly IMealRepository _mealRepository;
        private readonly IClientRepository _clientRepository;

        public ReportServiceImpl(IGoalsRepository goalsRepository, IDayRepository dayRepository, IHomePageService homePageService, IMealRepository mealRepository, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _mealRepository = mealRepository;
            _homePageService = homePageService;
            _dayRepository = dayRepository;
            _goalsRepository = goalsRepository;
        }

        private List<DayDTO> GetDays(int dayId)
        {
            var days = new List<DayDTO>();
            foreach (var item in GetMonthDays(dayId))
            {
                var goal = _goalsRepository.GetDayGoals(item.DayId);
                DateTime dateTime = _dayRepository.GetDayDateTime(item.DayId);
                days.Add(new DayDTO()
                {
                    Date = item.Date.GetValueOrDefault(),
                    KcalConsumed = _homePageService.SumAllKcalInDay(dateTime),
                    ProteinsConsumed = _homePageService.SumAllProteinsInDay(dateTime),
                    FatsConsumed = _homePageService.SumAllFatsInDay(dateTime),
                    CarbohydratesConsumed = _homePageService.SumAllCarbsInDay(dateTime),
                    KcalGoal = goal.Calories,
                    ProteinsGoal = goal.Proteins,
                    FatsGoal = goal.Fats,
                    CarbohydratesGoal = goal.Carbohydrates,
                    WaterDrunk = item.WaterDrunk.GetValueOrDefault()
                });
            }
            return days.OrderBy(d => d.Date).ToList();
        }

        private List<Models.Day> GetMonthDays(int dayId)
        {
            var date = _dayRepository.GetDay(dayId).Date.GetValueOrDefault();
            var list = new List<Models.Day>();
            foreach (var day in _dayRepository.GetLoggedInClientDays())
            {
                if (day.Date.GetValueOrDefault().Year == date.Year && day.Date.GetValueOrDefault().Month == date.Month)
                {
                    list.Add(day);
                }
            }
            return list;
        }

        public DaysSummaryDTO GetMonthSummary(int dayId)
        {
            var list = GetDays(dayId);

            return new DaysSummaryDTO()
            {
                Days = list,
                SumOfKcalConsumed = list.Sum(d => d.KcalConsumed),
                SumOfCarbsConsumed = list.Sum(d => d.CarbohydratesConsumed),
                SumOfFatsConsumed = list.Sum(d => d.FatsConsumed),
                SumOfProteinsConsumed = list.Sum(d => d.ProteinsConsumed),
                SumOfKcalGoals = list.Sum(d => d.KcalGoal),
                SumOfProteinsGoals = list.Sum(d => d.ProteinsGoal),
                SumOfCarbsGoals = list.Sum(d => d.CarbohydratesGoal),
                SumOfFatsGoals = list.Sum(d => d.FatsGoal),
                SumOfWaterDrunk = list.Sum(d => d.WaterDrunk)
            };
        }


        public DaySummaryWithDetailsDTO GetDayWithDetails(int dayId)
        {
            var goal = _goalsRepository.GetDayGoals(dayId);
            DateTime dateTime = _dayRepository.GetDayDateTime(dayId);
            var day = _dayRepository.GetDay(dayId);
            var meals = _mealRepository.GetAllDayMeals(dayId);

            return new DaySummaryWithDetailsDTO()
            {
                DaySummaryDTO = new DayDTO
                {
                    Date = day.Date.GetValueOrDefault(),
                    KcalConsumed = _homePageService.SumAllKcalInDay(dateTime),
                    ProteinsConsumed = _homePageService.SumAllProteinsInDay(dateTime),
                    FatsConsumed = _homePageService.SumAllFatsInDay(dateTime),
                    CarbohydratesConsumed = _homePageService.SumAllCarbsInDay(dateTime),
                    KcalGoal = goal.Calories,
                    ProteinsGoal = goal.Proteins,
                    FatsGoal = goal.Fats,
                    CarbohydratesGoal = goal.Carbohydrates,
                    WaterDrunk = day.WaterDrunk.GetValueOrDefault()
                },
                DetailsDTO = new DaySummaryDetailsDTO
                {
                    VitaminA_Consumed = meals.Sum(m => m.VitaminA),
                    VitaminB1_Consumed = meals.Sum(m => m.VitaminB1),
                    VitaminB2_Consumed = meals.Sum(m => m.VitaminB2),
                    VitaminB5_Consumed = meals.Sum(m => m.VitaminB5),
                    VitaminB6_Consumed = meals.Sum(m => m.VitaminB6),
                    VitaminB12_Consumed = meals.Sum(m => m.VitaminB12),
                    VitaminC_Consumed = meals.Sum(m => m.VitaminC),
                    VitaminD_Consumed = meals.Sum(m => m.VitaminD),
                    VitaminE_Consumed = meals.Sum(m => m.VitaminE),
                    VitaminK_Consumed = meals.Sum(m => m.VitaminK),
                    VitaminPp_Consumed = meals.Sum(m => m.VitaminPp),
                    FolicAcid_Consumed = meals.Sum(m => m.FolicAcid),
                    Biotin_Consumed = meals.Sum(m => m.Biotin),
                    Calcium_Consumed = meals.Sum(m => m.Calcium),
                    Copper_Consumed = meals.Sum(m => m.Copper),
                    Iodine_Consumed = meals.Sum(m => m.Iodine),
                    Iron_Consumed = meals.Sum(m => m.Iron),
                    Magnesium_Consumed = meals.Sum(m => m.Magnesium),
                    Phosphorus_Consumed = meals.Sum(m => m.Phosphorus),
                    Potassium_Consumed = meals.Sum(m => m.Potassium),
                    Selenium_Consumed = meals.Sum(m => m.Selenium),
                    Sodium_Consumed = meals.Sum(m => m.Sodium),
                    Zinc_Consumed = meals.Sum(m => m.Zinc),
                    VitaminA_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminA, 800d),
                    VitaminB1_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminB1, 1.1),
                    VitaminB2_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminB2, 1.4),
                    VitaminB5_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminB5, 7d),
                    VitaminB6_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminB6, 1.4),
                    VitaminB12_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminB12, 12d),
                    VitaminC_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminC, 80d),
                    VitaminD_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminD, 5d),
                    VitaminE_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminE, 12d),
                    VitaminK_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminK, 75d),
                    VitaminPp_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.VitaminPp, 16d),
                    FolicAcid_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.FolicAcid, 200d),
                    Biotin_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Biotin, 50d),
                    Calcium_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Calcium, 800d),
                    Copper_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Copper, 1d),
                    Iodine_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Iodine, 150d),
                    Iron_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Iron, 14d),
                    Magnesium_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Magnesium, 375d),
                    Phosphorus_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Phosphorus, 700d),
                    Potassium_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Potassium, 2000d),
                    Selenium_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Selenium, 55d),
                    Sodium_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Sodium, 155d),
                    Zinc_Goal = ReturnIfNotNullOrMultiplyByTheFactor(goal.Zinc, 10d)
                }
            };
        }

        private double ReturnIfNotNullOrMultiplyByTheFactor(double? value, double var)
        {
            return ReturnIfNotNull(value, MultiplyByTheFactor(var));
        }

        private double ReturnIfNotNull(double? value, double valueIfNull)
        {
            return (double)(value == null ? valueIfNull : value);
        }

        private double MultiplyByTheFactor(double var)
        {
            return var * ((double)_clientRepository.GetLoggedInClient().CaloricDemand / 2000d);
        }
    }
}
