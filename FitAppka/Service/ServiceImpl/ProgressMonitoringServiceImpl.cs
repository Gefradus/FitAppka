using AutoMapper;
using FitAppka.Models;
using FitAppka.Models.Enum;
using FitAppka.Repository;
using FitAppka.Strategy;
using FitAppka.Strategy.StrategyInterface;


namespace FitAppka.Service.ServiceImpl
{
    public class ProgressMonitoringServiceImpl : IProgressMonitoringService
    {
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
            new ChartTypeStrategyDictionary<IChartTypeStrategy>(_clientRepository, _goalsService, _dayRepository, _weightMeasurementRepository,
                _homePageService, _mapper, _fatMeasurementRepository, _cardioService).TryGetValue((ChartStrategyEnum)chartType, out IChartTypeStrategy mapValue);

            return mapValue.GetChartDataList(dateFrom, dateTo);
        }
    }
}
