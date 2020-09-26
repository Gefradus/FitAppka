using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class CaloriesBurnedChartStrategy : IChartTypeStrategy
    {
        private readonly IGoalsService _goalsService;
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;

        public CaloriesBurnedChartStrategy(IClientRepository clientRepository, IGoalsService goalsService, IDayRepository dayRepository)
        {
            _goalsService = goalsService;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.CaloriesBurned,
                ChartDataInDays = GetCaloriesBurnedInDaysFromTo(dateFrom, dateTo)
            };
        }

        private List<ChartDataInDayDTO> GetCaloriesBurnedInDaysFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<ChartDataInDayDTO>();

            foreach (var item in _dayRepository.GetClientDays(_clientRepository.GetLoggedInClientId()))
            {
                DateTime day = item.Date.GetValueOrDefault().Date;
                if (day <= dateTimeTo && day >= dateTimeFrom)
                {
                    list.Add(new ChartDataInDayDTO()
                    {
                        DateOfDay = day,
                        ChartData = _goalsService.CaloriesBurnedInDay(item.DayId),
                        ChartDataGoal = _goalsService.GetDayGoals(item.DayId).KcalBurned.GetValueOrDefault()
                    });
                }
            }
            return list;
        }
    }
}
