using FitnessApp.Models.DTO;

namespace FitnessApp.Service.ServiceImpl
{
    public class TrainingPanelServiceImpl : ITrainingPanelService
    {
        private readonly ICardioTrainingService _cardioServices;
        private readonly IStrengthTrainingService _strengthTrainingService;
        private readonly IGoalsService _goalsService;
        private readonly IDayManageService _dayService;
        private readonly IClientManageService _clientManageService;
        
        public TrainingPanelServiceImpl(ICardioTrainingService cardioServices, IGoalsService goalsService, IDayManageService dayService, 
            IClientManageService clientManageService, IStrengthTrainingService strengthTrainingService)
        {
            _clientManageService = clientManageService;
            _cardioServices = cardioServices;
            _strengthTrainingService = strengthTrainingService;
            _goalsService = goalsService;
            _dayService = dayService;
        }

        public TrainingPanelDTO Dto (int? dayId, int? cardioPage, int? strengthPage) {
            int dayID = (int)(dayId == null ? _dayService.GetTodayId() : dayId);
            return new TrainingPanelDTO()
            {
                DayId = dayID,
                Day = _dayService.GetDayDateTime(dayID).Date.ToString("dd.MM.yyyy"),
                ClientId = _clientManageService.GetLoggedInClientId(),
                BurnedKcal = _goalsService.CaloriesBurnedInDay(dayID),
                CardioTime = _cardioServices.GetCardioTimeInDay(dayID),
                KcalTarget = _cardioServices.GetKcalBurnedGoalInDay(dayID),
                TimeTarget = _cardioServices.GetTrainingTimeGoalInDay(dayID),
                StrengthTrainings = _strengthTrainingService.GetStrengthTrainingsInDay(dayID, strengthPage),
                CardioTrainings = _cardioServices.GetCardioTrainingsInDay(dayID, cardioPage),
                CardioTrainingsPage = cardioPage,
                StrengthTrainingsPage = strengthPage
            };
        }
    }
}
