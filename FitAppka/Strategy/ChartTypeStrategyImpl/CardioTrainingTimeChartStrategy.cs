using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class CardioTrainingTimeChartStrategy : IChartTypeStrategy
    {
        private readonly ICardioTrainingService _cardioService;
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;

        public CardioTrainingTimeChartStrategy(IClientRepository clientRepository, ICardioTrainingService cardioService,
            IDayRepository dayRepository)
        {
            _cardioService = cardioService;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.CardioTrainingTime,
                CaloriesInDays = GetCaloriesBurnedInDaysFromTo(dateFrom, dateTo)
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
                        ChartData = _cardioService.GetCardioTimeInDay(item.DayId).GetValueOrDefault(),
                        ChartDataGoal = _cardioService.GetTrainingTimeGoalInDay(item.DayId).GetValueOrDefault()
                    });
                }
            }
            return list;
        }
    }
}
