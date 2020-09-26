using FitAppka.Models.Enum;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using System.Collections.Generic;
using AutoMapper;
using FitAppka.Repository;
using FitAppka.Service;
using System;

namespace FitAppka.Strategy
{
    public class ChartTypeStrategyDictionary<T> : Dictionary<ChartStrategyEnum, T>
    {
        public ChartTypeStrategyDictionary(IClientRepository clientRepository, IGoalsService goalsService, IDayRepository dayRepository, 
            IWeightMeasurementRepository weightMeasurementRepository,IHomePageService homePageService, IMapper mapper,
            IFatMeasurementRepository fatMeasurementRepository, ICardioTrainingService cardioService)
        {
            Add(ChartStrategyEnum.CaloriesBurned, ConvertToT(new CaloriesBurnedChartStrategy(clientRepository, goalsService, dayRepository)));
            Add(ChartStrategyEnum.CaloriesConsumed, ConvertToT(new CaloriesConsumedChartStrategy(dayRepository, homePageService, clientRepository, goalsService)));
            Add(ChartStrategyEnum.CardioTrainingTime, ConvertToT(new CardioTrainingTimeChartStrategy(clientRepository, cardioService, dayRepository)));
            Add(ChartStrategyEnum.EstimatedBodyFat, ConvertToT(new EstimatedBodyFatChartStrategy(fatMeasurementRepository, weightMeasurementRepository)));
            Add(ChartStrategyEnum.WaistCircumference, ConvertToT(new WaistCircumferenceMeasurementChartStrategy(fatMeasurementRepository, weightMeasurementRepository)));
            Add(ChartStrategyEnum.WaterConsumption, ConvertToT(new WaterConsumptionChartStrategy(dayRepository, clientRepository, goalsService)));
            Add(ChartStrategyEnum.WeightMeasurement, ConvertToT(new WeightMeasurementChartStrategy(weightMeasurementRepository, mapper)));
        }

        private static T ConvertToT<O>(O o)
        {
            return (T)Convert.ChangeType(o, typeof(O));
        }
    }
}
