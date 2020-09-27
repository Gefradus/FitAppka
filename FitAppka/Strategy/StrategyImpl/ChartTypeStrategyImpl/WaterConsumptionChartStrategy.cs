using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Service;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class WaterConsumptionChartStrategy : IChartTypeStrategy
    {
        public IDayRepository DayRepository { get; set; }
        public IGoalsService GoalsService { get; set; }

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
