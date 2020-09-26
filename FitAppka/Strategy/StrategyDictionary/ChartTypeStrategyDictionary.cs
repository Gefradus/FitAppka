using FitAppka.Models.Enum;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using System.Collections.Generic;
using System;
using FitAppka.Service.ServiceImpl;
using AutoMapper;

namespace FitAppka.Strategy
{
    public class ChartTypeStrategyDictionary<T> : Dictionary<ChartStrategyEnum, T>
    {
        private readonly ProgressMonitoringServiceImpl serviceImpl;
        private readonly IMapper _mapper;

        public ChartTypeStrategyDictionary(ProgressMonitoringServiceImpl service, IMapper mapper)
        {
            _mapper = mapper;
            serviceImpl = service;
            Add(ChartStrategyEnum.CaloriesBurned, ConvertToT(new CaloriesBurnedChartStrategy()));
            Add(ChartStrategyEnum.CaloriesConsumed, ConvertToT(new CaloriesConsumedChartStrategy()));
            Add(ChartStrategyEnum.CardioTrainingTime, ConvertToT(new CardioTrainingTimeChartStrategy()));
            Add(ChartStrategyEnum.EstimatedBodyFat, ConvertToT(new EstimatedBodyFatChartStrategy()));
            Add(ChartStrategyEnum.WaistCircumference, ConvertToT(new WaistCircumferenceMeasurementChartStrategy()));
            Add(ChartStrategyEnum.WaterConsumption, ConvertToT(new WaterConsumptionChartStrategy()));
            Add(ChartStrategyEnum.WeightMeasurement, ConvertToT(new WeightMeasurementChartStrategy()));
        }

        private T ConvertToT<O>(O o) {
            return (T)Convert.ChangeType(_mapper.Map(serviceImpl, o), typeof(O));
        }
    }
}
