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
        public IWeightMeasurementRepository WeightMeasurementRepository { get;set; }
        public IFatMeasurementRepository FatMeasurementRepository { get; set; }
        public IDayRepository DayRepository { get; set; }
        public IClientRepository ClientRepository { get; set; }
        public IHomePageService HomePageService { get; set; }
        public IGoalsService GoalsService { get; set; }
        public ICardioTrainingService CardioService { get; set; }
        private readonly IMapper _mapper;

        public ProgressMonitoringServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IGoalsService goalsService,
            IDayRepository dayRepository, IHomePageService homePageService, IClientRepository clientRepository, IMapper mapper,
            IFatMeasurementRepository fatMeasurementRepository, ICardioTrainingService cardioService)
        {
            _mapper = mapper;
            CardioService = cardioService;
            FatMeasurementRepository = fatMeasurementRepository;
            GoalsService = goalsService;
            DayRepository = dayRepository;
            HomePageService = homePageService;
            ClientRepository = clientRepository;
            WeightMeasurementRepository = weightMeasurementRepository;
        }

        public ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType)
        {
            new ChartTypeStrategyDictionary<IChartTypeStrategy>(this, _mapper).
                TryGetValue((ChartStrategyEnum)chartType, out IChartTypeStrategy mapValue);

            return mapValue.GetChartDataList(dateFrom, dateTo);
        }
    }
}
