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
            foreach(var day in _dayRepository.GetLoggedInClientDays())
            {
                if(day.Date.GetValueOrDefault().Year == date.Year && day.Date.GetValueOrDefault().Month == date.Month) {
                    list.Add(day);
                }
            }
            return list;
        }

        public DaysSummaryDTO GetMonthSummary(int dayId) {
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
                    VitaminA_Goal = MultiplyByTheFactor(800d),
                    VitaminB1_Goal = MultiplyByTheFactor(1.1),
                    VitaminB2_Goal = MultiplyByTheFactor(1.4),
                    VitaminB5_Goal = MultiplyByTheFactor(7d),
                    VitaminB6_Goal = MultiplyByTheFactor(1.4),
                    VitaminB12_Goal = MultiplyByTheFactor(12d),
                    VitaminC_Goal = MultiplyByTheFactor(80d),
                    VitaminD_Goal = MultiplyByTheFactor(5d),
                    VitaminE_Goal = MultiplyByTheFactor(12d),
                    VitaminK_Goal = MultiplyByTheFactor(75d),
                    VitaminPp_Goal = MultiplyByTheFactor(16d),
                    FolicAcid_Goal = MultiplyByTheFactor(200d),
                    Biotin_Goal = MultiplyByTheFactor(50d),
                    Calcium_Goal = MultiplyByTheFactor(800d),
                    Copper_Goal = MultiplyByTheFactor(1d),
                    Iodine_Goal = MultiplyByTheFactor(150d),
                    Iron_Goal = MultiplyByTheFactor(14d),
                    Magnesium_Goal = MultiplyByTheFactor(375d),
                    Phosphorus_Goal = MultiplyByTheFactor(700d),
                    Potassium_Goal = MultiplyByTheFactor(2000d),
                    Selenium_Goal = MultiplyByTheFactor(55d),
                    Sodium_Goal = MultiplyByTheFactor(155d),
                    Zinc_Goal = MultiplyByTheFactor(10d)
                }
            };
        }



        private double MultiplyByTheFactor(double var)
        {
            return (int)(var * ((double)_clientRepository.GetLoggedInClient().CaloricDemand / 2000d));
        }
    }
}
