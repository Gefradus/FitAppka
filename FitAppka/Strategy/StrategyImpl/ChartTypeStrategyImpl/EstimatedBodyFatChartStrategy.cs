using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitnessApp.Strategy.ChartTypeStrategyImpl
{
    public class EstimatedBodyFatChartStrategy : IChartTypeStrategy
    {
        public IFatMeasurementRepository FatMeasurementRepository { private get; set; }
        public IWeightMeasurementRepository WeightMeasurementRepository { private get; set; }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartTypeStrategyEnum.EstimatedBodyFat,
                Measurements = EstimatedBodyFatListFromTo(dateFrom, dateTo)
            };
        }

        private List<MeasurementDTO> EstimatedBodyFatListFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);

            var list = new List<MeasurementDTO>();
            foreach (var item in FatMeasurementRepository.GetLoggedInClientFatMeasurements())
            {
                var dateFromMeasurement = SelectDateFromFatMeasurement(item);
                if (dateFromMeasurement <= dateTimeTo && dateFromMeasurement >= dateTimeFrom)
                {
                    list.Add(new MeasurementDTO()
                    {
                        DateOfMeasurement = dateFromMeasurement,
                        Measurement = item.BodyFatLevel.GetValueOrDefault()
                    });
                }
            }
            return DateConverter.SortByMeasurementDate(list);
        }


        private DateTime SelectDateFromFatMeasurement(FatMeasurement measurement)
        {
            return WeightMeasurementRepository.GetWeightMeasurement(measurement.WeightMeasurementId)
                .DateOfMeasurement.GetValueOrDefault().Date;
        }

    }
}
