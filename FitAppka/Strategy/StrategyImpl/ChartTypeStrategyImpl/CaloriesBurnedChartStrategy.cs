using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Service;
using FitnessApp.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.ChartTypeStrategyImpl
{
    public class CaloriesBurnedChartStrategy : IChartTypeStrategy
    {
        public IGoalsService GoalsService { private get; set; }
        public IDayRepository DayRepository { private get; set; }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartTypeStrategyEnum.CaloriesBurned,
                ChartDataInDays = GetCaloriesBurnedInDaysFromTo(dateFrom, dateTo)
            };
        }

        private List<ChartDataInDayDTO> GetCaloriesBurnedInDaysFromTo(string dateFrom, string dateTo)
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
                        ChartData = GoalsService.CaloriesBurnedInDay(item.DayId),
                        ChartDataGoal = GoalsService.GetDayGoals(item.DayId).KcalBurned.GetValueOrDefault()
                    });
                }
            }
            return DateConverter.SortChartDataByDate(list);
        }
    }
}
