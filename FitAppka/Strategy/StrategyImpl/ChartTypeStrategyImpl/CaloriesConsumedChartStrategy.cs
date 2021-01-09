using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Service;
using FitnessApp.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.ChartTypeStrategyImpl
{
    public class CaloriesConsumedChartStrategy : IChartTypeStrategy
    {
        public IDayRepository DayRepository { private get; set; }
        public IHomePageService HomePageService { private get; set; }
        public IGoalsService GoalsService { private get; set; }


        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartTypeStrategyEnum.CaloriesConsumed,
                ChartDataInDays = GetCaloriesConsumedInDaysFromTo(dateFrom, dateTo),
            };
        }

        private List<ChartDataInDayDTO> GetCaloriesConsumedInDaysFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<ChartDataInDayDTO>();

            foreach (var item in DayRepository.GetLoggedInClientDays())
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
            return DateConverter.SortChartDataByDate(list);
        }
    }
}
