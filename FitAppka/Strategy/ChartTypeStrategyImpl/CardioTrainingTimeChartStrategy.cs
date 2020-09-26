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
        public ICardioTrainingService CardioService { get; set; }
        public IClientRepository ClientRepository { get; set; }
        public IDayRepository DayRepository { get; set; }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.CardioTrainingTime,
                ChartDataInDays = GetCaloriesBurnedInDaysFromTo(dateFrom, dateTo)
            };
        }

        private List<ChartDataInDayDTO> GetCaloriesBurnedInDaysFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<ChartDataInDayDTO>();

            foreach (var item in DayRepository.GetClientDays(ClientRepository.GetLoggedInClientId()))
            {
                DateTime day = item.Date.GetValueOrDefault().Date;
                if (day <= dateTimeTo && day >= dateTimeFrom)
                {
                    list.Add(new ChartDataInDayDTO()
                    {
                        DateOfDay = day,
                        ChartData = CardioService.GetCardioTimeInDay(item.DayId).GetValueOrDefault(),
                        ChartDataGoal = CardioService.GetTrainingTimeGoalInDay(item.DayId).GetValueOrDefault()
                    });
                }
            }
            return list;
        }
    }
}
