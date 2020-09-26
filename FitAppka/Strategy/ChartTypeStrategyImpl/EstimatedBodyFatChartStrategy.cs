using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class EstimatedBodyFatChartStrategy : IChartTypeStrategy
    {
        private readonly IFatMeasurementRepository _fatMeasurementRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        public ChartStrategyEnum ChartStrategyEnum { get; set; }

        public EstimatedBodyFatChartStrategy(IFatMeasurementRepository fatMeasurementRepository,
            IWeightMeasurementRepository weightMeasurementRepository)
        {
            _weightMeasurementRepository = weightMeasurementRepository;
            _fatMeasurementRepository = fatMeasurementRepository;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.EstimatedBodyFat,
                Measurements = EstimatedBodyFatListFromTo(dateFrom, dateTo)
            };
        }

        private List<MeasurementDTO> EstimatedBodyFatListFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);

            var list = new List<MeasurementDTO>();
            foreach (var item in _fatMeasurementRepository.GetLoggedInClientFatMeasurements())
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
            return list;
        }


        private DateTime SelectDateFromFatMeasurement(FatMeasurement measurement)
        {
            return _weightMeasurementRepository.GetWeightMeasurement(measurement.WeightMeasurementId)
                .DateOfMeasurement.GetValueOrDefault().Date;
        }

    }
}
