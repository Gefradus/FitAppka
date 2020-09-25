using FitAppka.Models;
using FitAppka.Models.DTO;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class CaloriesConsumedChartStrategy : IChartTypeStrategy
    {

        private readonly IDayRepository _dayRepository;
        private readonly IHomePageService _homePageService;
        private readonly IClientRepository _clientRepository;
        private readonly IGoalsService _goalsService;

        public CaloriesConsumedChartStrategy(IDayRepository dayRepository, IHomePageService homePageService, 
            IClientRepository clientRepository, IGoalsService goalsService)
        {
            _goalsService = goalsService;
            _dayRepository = dayRepository;
            _homePageService = homePageService;
            _clientRepository = clientRepository;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = '1',
                CaloriesInDays = GetCaloriesConsumedInDaysFromTo(dateFrom, dateTo),
            };
        }

        private List<CaloriesInDayDTO> GetCaloriesConsumedInDaysFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<CaloriesInDayDTO>();

            foreach (var item in _dayRepository.GetClientDays(_clientRepository.GetLoggedInClientId()))
            {
                DateTime day = item.Date.GetValueOrDefault().Date;
                if (day <= dateTimeTo && day >= dateTimeFrom)
                {
                    list.Add(new CaloriesInDayDTO()
                    {
                        DateOfDay = day,
                        Calories = (int)_homePageService.SumAllKcalInDay(day),
                        CaloriesGoal = (int)_goalsService.GetDayGoals(item.DayId).Calories
                    });
                }
            }
            return list;
        }
    }
}
