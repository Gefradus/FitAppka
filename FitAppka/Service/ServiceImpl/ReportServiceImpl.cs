using FitAppka.Models.DTO;
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

        public List<DaySummaryDTO> GetDays()
        {
            var days = new List<DaySummaryDTO>();
            foreach (var item in _dayRepository.GetLoggedInClientDays())
            {
                var goal = _goalsRepository.GetDayGoals(item.DayId);
                DateTime dateTime = _dayRepository.GetDayDateTime(item.DayId);
                days.Add(new DaySummaryDTO()
                {
                    Date = item.Date.GetValueOrDefault(),
                    KcalConsumed = (int)_homePageService.SumAllKcalInDay(dateTime),
                    ProteinsConsumed = _homePageService.SumAllProteinsInDay(dateTime),
                    FatsConsumed = _homePageService.SumAllFatsInDay(dateTime),
                    CarbohydratesConsumed = _homePageService.SumAllCarbsInDay(dateTime),
                    KcalGoal = (int)goal.Calories,
                    ProteinsGoal = goal.Proteins,
                    FatsGoal = goal.Fats,
                    CarbohydratesGoal = goal.Carbohydrates,
                    WaterDrunk = item.WaterDrunk.GetValueOrDefault()
                });
            }
            return days.OrderBy(d => d.Date).ToList();
        }

        public DaySummaryWithDetailsDTO GetDayWithDetails()
        {
            throw new System.NotImplementedException();
        }
    }
}
