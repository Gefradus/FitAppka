using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Strategy.StrategyInterface;
using System.Collections.Generic;

namespace FitnessApp.Strategy.ChartTypeStrategyImpl
{
    public class WeightMeasurementChartStrategy : IChartTypeStrategy
    {
        public IWeightMeasurementRepository WeightMeasurementRepository { private get; set; }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartTypeStrategyEnum.WeightMeasurement,
                Measurements = GetWeightMeasurementListFromTo(dateFrom, dateTo)
            };
        }

        private List<MeasurementDTO> GetWeightMeasurementListFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<MeasurementDTO>();

            foreach (var measurement in WeightMeasurementRepository.GetLoggedInClientWeightMeasurements())
            {
                var measurementDate = measurement.DateOfMeasurement.GetValueOrDefault().Date;
                if (measurementDate <= dateTimeTo && measurementDate >= dateTimeFrom)
                {
                    list.Add(new MeasurementDTO() { 
                        DateOfMeasurement = measurementDate,
                        Measurement = measurement.Weight
                    });
                }
            }
            
            return DateConverter.SortByMeasurementDate(list);
        }
    }
}
