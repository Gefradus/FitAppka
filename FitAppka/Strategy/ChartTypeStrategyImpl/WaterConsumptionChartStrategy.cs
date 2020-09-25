using FitAppka.Models;
using FitAppka.Models.DTO;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class WaterConsumptionChartStrategy : IChartTypeStrategy
    {

        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IGoalsService _goalsService;

        public WaterConsumptionChartStrategy(IDayRepository dayRepository, IClientRepository clientRepository, IGoalsService goalsService)
        {
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
            _goalsService = goalsService;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = '3',
                CaloriesInDays = GetWaterConsumedInDaysFromTo(dateFrom, dateTo),
            };
        }

        private List<ChartDataInDayDTO> GetWaterConsumedInDaysFromTo(string dateFrom, string dateTo)
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
                        ChartData = item.WaterDrunk.GetValueOrDefault(),
                        ChartDataGoal = (int)_goalsService.GetDayGoals(item.DayId).Calories
                    });
                }
            }
            return list;
        }

    }
}
