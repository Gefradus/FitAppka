using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using FitAppka.Strategy.StrategyInterface;


namespace FitAppka.Service.ServiceImpl
{
    public class ProgressMonitoringServiceImpl : IProgressMonitoringService
    {
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IHomePageService _homePageService;
        private readonly IGoalsService _goalsService;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private IChartTypeStrategy _chartTypeStrategy;

        public ProgressMonitoringServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IGoalsService goalsService,
            IDayRepository dayRepository, IHomePageService homePageService, IClientRepository clientRepository, IMapper mapper)
        {
            _goalsService = goalsService;
            _dayRepository = dayRepository;
            _homePageService = homePageService;
            _clientRepository = clientRepository;
            _weightMeasurementRepository = weightMeasurementRepository;
            _mapper = mapper;
        }

        public ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType)
        {
            if(chartType == 0) {
                _chartTypeStrategy = new WeightMeasurementChartStrategy(_weightMeasurementRepository, _mapper);
            }
            if(chartType == 1) {
                _chartTypeStrategy = new CaloriesConsumedChartStrategy(_dayRepository, _homePageService, _clientRepository);
            }
            if(chartType == 2) {
                _chartTypeStrategy = new CaloriesBurnedChartStrategy(_clientRepository, _goalsService, _dayRepository);
            }

            return _chartTypeStrategy.GetChartDataList(dateFrom, dateTo);
        }
    }
}
