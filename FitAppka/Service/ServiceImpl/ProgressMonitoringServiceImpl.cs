using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Models.Enum;
using FitnessApp.Repository;
using FitnessApp.Strategy;
using FitnessApp.Strategy.StrategyInterface;

namespace FitnessApp.Service.ServiceImpl
{
    public class ProgressMonitoringServiceImpl : IProgressMonitoringService
    {
        public IWeightMeasurementRepository WeightMeasurementRepository { get; private set; }
        public IFatMeasurementRepository FatMeasurementRepository { get; private set; }
        public IDayRepository DayRepository { get; private set; }
        public IHomePageService HomePageService { get; private set; }
        public IGoalsService GoalsService { get; private set; }
        public ICardioTrainingService CardioService { get; private set; }
        private readonly IMapper _mapper;

        public ProgressMonitoringServiceImpl(IWeightMeasurementRepository weightMeasurementRepository, IGoalsService goalsService,
            IDayRepository dayRepository, IHomePageService homePageService, IMapper mapper, ICardioTrainingService cardioService,
            IFatMeasurementRepository fatMeasurementRepository)
        {
            _mapper = mapper;
            CardioService = cardioService;
            FatMeasurementRepository = fatMeasurementRepository;
            GoalsService = goalsService;
            DayRepository = dayRepository;
            HomePageService = homePageService;
            WeightMeasurementRepository = weightMeasurementRepository;
        }

        public ProgressMonitoringDTO GetProgressMonitoringDTO(string dateFrom, string dateTo, int chartType)
        {
            new ChartTypeStrategyDictionary<IChartTypeStrategy>(this, _mapper).
                TryGetValue((ChartTypeStrategyEnum)chartType, out IChartTypeStrategy mapValue);

            return mapValue.GetChartDataList(dateFrom, dateTo);
        }
    }
}
