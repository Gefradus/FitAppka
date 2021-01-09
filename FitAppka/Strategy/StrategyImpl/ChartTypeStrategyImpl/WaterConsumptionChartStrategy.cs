using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Service;
using FitnessApp.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.ChartTypeStrategyImpl
{
    public class WaterConsumptionChartStrategy : IChartTypeStrategy
    {
        public IDayRepository DayRepository { private get; set; }
        public IGoalsService GoalsService { private get; set; }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartTypeStrategyEnum.WaterConsumption,
                ChartDataInDays = GetWaterConsumedInDaysFromTo(dateFrom, dateTo),
            };
        }

        private List<ChartDataInDayDTO> GetWaterConsumedInDaysFromTo(string dateFrom, string dateTo)
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
                        ChartData = item.WaterDrunk.GetValueOrDefault(),
                        ChartDataGoal = (int)GoalsService.GetDayGoals(item.DayId).Calories
                    });
                }
            }
            return DateConverter.SortChartDataByDate(list);
        }

    }
}
