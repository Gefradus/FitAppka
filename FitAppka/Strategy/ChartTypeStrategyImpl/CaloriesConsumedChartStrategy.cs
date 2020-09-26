using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class CaloriesConsumedChartStrategy : IChartTypeStrategy
    {
        public IDayRepository DayRepository { get; set; }
        public IHomePageService HomePageService { get; set; }
        public IClientRepository ClientRepository { get; set; }
        public IGoalsService GoalsService { get; set; }


        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.CaloriesConsumed,
                ChartDataInDays = GetCaloriesConsumedInDaysFromTo(dateFrom, dateTo),
            };
        }

        private List<ChartDataInDayDTO> GetCaloriesConsumedInDaysFromTo(string dateFrom, string dateTo)
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
                        ChartData = (int)HomePageService.SumAllKcalInDay(day),
                        ChartDataGoal = (int)GoalsService.GetDayGoals(item.DayId).Calories
                    });
                }
            }
            return list;
        }
    }
}
