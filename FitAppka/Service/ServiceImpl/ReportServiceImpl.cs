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

        public ReportServiceImpl(IGoalsRepository goalsRepository, IDayRepository dayRepository, IHomePageService homePageService)
        {
            _homePageService = homePageService;
            _dayRepository = dayRepository;
            _goalsRepository = goalsRepository;
        }

        private List<DayDTO> GetDays()
        {
            var days = new List<DayDTO>();
            foreach (var item in _dayRepository.GetLoggedInClientDays())
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

        public DaysSummaryDTO GetDaysSummary() {
            var list = GetDays();

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
                SumOfFatsGoals = list.Sum(d => d.FatsGoal)
            };
        }


        public DaySummaryWithDetailsDTO GetDayWithDetails()
        {
            throw new System.NotImplementedException();
        }
    }
}
