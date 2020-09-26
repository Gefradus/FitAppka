using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Strategy.StrategyInterface;
using System.Collections.Generic;

namespace FitAppka.Strategy.ChartTypeStrategyImpl
{
    public class WeightMeasurementChartStrategy : IChartTypeStrategy
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IMapper _mapper;

        public WeightMeasurementChartStrategy(IWeightMeasurementRepository weightMeasurementRepository, IMapper mapper)
        {
            _mapper = mapper;
            _weightMeasurementRepository = weightMeasurementRepository;
        }

        public ProgressMonitoringDTO GetChartDataList(string dateFrom, string dateTo)
        {
            return new ProgressMonitoringDTO()
            {
                DateFrom = DateConverter.ConvertToJSDate(dateFrom, true),
                DateTo = DateConverter.ConvertToJSDate(dateTo, false),
                ChartType = ChartStrategyEnum.WeightMeasurement,
                Measurements = GetWeightMeasurementListFromTo(dateFrom, dateTo)
            };
        }

        private List<MeasurementDTO> GetWeightMeasurementListFromTo(string dateFrom, string dateTo)
        {
            var dateTimeFrom = DateConverter.ConvertToDateTimeAndPreventNull(dateFrom, true);
            var dateTimeTo = DateConverter.ConvertToDateTimeAndPreventNull(dateTo, false);
            var list = new List<MeasurementDTO>();

            foreach (var measurement in _weightMeasurementRepository.GetLoggedInClientWeightMeasurements())
            {
                if (measurement.DateOfMeasurement.GetValueOrDefault().Date <= dateTimeTo && measurement.DateOfMeasurement.GetValueOrDefault().Date >= dateTimeFrom)
                {
                    list.Add(_mapper.Map<WeightMeasurement, MeasurementDTO>(measurement));
                }
            }
            return list;
        }
    }
}
