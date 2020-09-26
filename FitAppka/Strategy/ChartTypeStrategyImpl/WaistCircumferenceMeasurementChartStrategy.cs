using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class WaistCircumferenceMeasurementChartStrategy : IChartTypeStrategy
    {
        private readonly IFatMeasurementRepository _fatMeasurementRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        public ChartStrategyEnum ChartStrategyEnum { get; set; }

        public WaistCircumferenceMeasurementChartStrategy(IFatMeasurementRepository fatMeasurementRepository,
            IWeightMeasurementRepository weightMeasurementRepository)
        {
            _weightMeasurementRepository = weightMeasurementRepository;
            _fatMeasurementRepository = fatMeasurementRepository;
            ChartStrategyEnum = ChartStrategyEnum.WaistCircumference;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.WaistCircumference,
                Measurements = GetWaistCircumferenceMeasurementListFromTo(dateFrom, dateTo)
            };
        }

        private List<MeasurementDTO> GetWaistCircumferenceMeasurementListFromTo(string dateFrom, string dateTo)
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
                        Measurement = item.WaistCircumference.GetValueOrDefault()
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
