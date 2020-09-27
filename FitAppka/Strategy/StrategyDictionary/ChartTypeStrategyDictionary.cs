using FitAppka.Models.Enum;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using System.Collections.Generic;
using System;
using FitAppka.Service.ServiceImpl;
using AutoMapper;

namespace FitAppka.Strategy
{
    public class ChartTypeStrategyDictionary<T> : Dictionary<ChartTypeStrategyEnum, T>
    {
        private readonly ProgressMonitoringServiceImpl serviceImpl;
        private readonly IMapper _mapper;

        public ChartTypeStrategyDictionary(ProgressMonitoringServiceImpl service, IMapper mapper)
        {
            _mapper = mapper;
            serviceImpl = service;
            Add(ChartTypeStrategyEnum.CaloriesBurned, ConvertToT(new CaloriesBurnedChartStrategy()));
            Add(ChartTypeStrategyEnum.CaloriesConsumed, ConvertToT(new CaloriesConsumedChartStrategy()));
            Add(ChartTypeStrategyEnum.CardioTrainingTime, ConvertToT(new CardioTrainingTimeChartStrategy()));
            Add(ChartTypeStrategyEnum.EstimatedBodyFat, ConvertToT(new EstimatedBodyFatChartStrategy()));
            Add(ChartTypeStrategyEnum.WaistCircumference, ConvertToT(new WaistCircumferenceMeasurementChartStrategy()));
            Add(ChartTypeStrategyEnum.WaterConsumption, ConvertToT(new WaterConsumptionChartStrategy()));
            Add(ChartTypeStrategyEnum.WeightMeasurement, ConvertToT(new WeightMeasurementChartStrategy()));
        }

        private T ConvertToT<Strategy>(Strategy s) {
            return (T)Convert.ChangeType(_mapper.Map(serviceImpl, s), typeof(Strategy));
        }
    }
}
