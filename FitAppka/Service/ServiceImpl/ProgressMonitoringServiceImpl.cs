using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using FitAppka.Strategy.StrategyInterface;


namespace FitAppka.Service.ServiceImpl
{
    public class ProgressMonitoringServiceImpl : IProgressMonitoringService
    {
        private IChartTypeStrategy _chartTypeStrategy;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IFatMeasurementRepository _fatMeasurementRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHomePageService _homePageService;
        private readonly IGoalsService _goalsService;
        private readonly ICardioTrainingService _cardioService;
        private readonly IMapper _mapper;

        public ProgressMonitoringServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IGoalsService goalsService,
            IDayRepository dayRepository, IHomePageService homePageService, IClientRepository clientRepository, IMapper mapper,
            IFatMeasurementRepository fatMeasurementRepository, ICardioTrainingService cardioService)
        {
            _cardioService = cardioService;
            _fatMeasurementRepository = fatMeasurementRepository;
            _goalsService = goalsService;
            _dayRepository = dayRepository;
            _homePageService = homePageService;
            _clientRepository = clientRepository;
            _weightMeasurementRepository = weightMeasurementRepository;
            _mapper = mapper;
        }

        public ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType)
        {
            if (chartType == (int) ChartStrategyEnum.CaloriesConsumed) {
                _chartTypeStrategy = new CaloriesConsumedChartStrategy(_dayRepository, _homePageService, _clientRepository, _goalsService);
            }
            if(chartType == (int) ChartStrategyEnum.CaloriesBurned) {
                _chartTypeStrategy = new CaloriesBurnedChartStrategy(_clientRepository, _goalsService, _dayRepository);
            }
            if (chartType == (int) ChartStrategyEnum.WeightMeasurement) {
                _chartTypeStrategy = new WeightMeasurementChartStrategy(_weightMeasurementRepository, _mapper);
            }
            if(chartType == (int) ChartStrategyEnum.WaistCircumference) {
                _chartTypeStrategy = new WaistCircumferenceMeasurementChartStrategy(_fatMeasurementRepository, _weightMeasurementRepository);
            }
            if(chartType == (int) ChartStrategyEnum.WaterConsumption) {
                _chartTypeStrategy = new WaterConsumptionChartStrategy(_dayRepository, _clientRepository, _goalsService);
            } 
            if(chartType == (int) ChartStrategyEnum.CardioTrainingTime) {
                _chartTypeStrategy = new CardioTrainingTimeChartStrategy(_clientRepository, _cardioService, _dayRepository);
            }
            if(chartType == (int) ChartStrategyEnum.EstimatedBodyFat) {
                _chartTypeStrategy = new EstimatedBodyFatChartStrategy(_fatMeasurementRepository, _weightMeasurementRepository);
            }

            return _chartTypeStrategy.GetChartDataList(dateFrom, dateTo);
        }
    }
}
