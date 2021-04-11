using FitnessApp.Models;
using FitnessApp.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FitnessApp.Service.ServiceImpl
{
    public class TrainingPanelServiceImpl : ITrainingPanelService
    {
        private readonly ICardioTrainingService _cardioServices;
        private readonly IStrengthTrainingService _strengthTrainingService;
        private readonly IGoalsService _goalsService;
        private readonly IDayManageService _dayService;
        private readonly IClientManageService _clientManageService;
        private readonly FitAppContext _context;

        public TrainingPanelServiceImpl(ICardioTrainingService cardioServices, IGoalsService goalsService, IDayManageService dayService, 
            IClientManageService clientManageService, FitAppContext context)
        {
            _clientManageService = clientManageService;
            _cardioServices = cardioServices;
            _goalsService = goalsService;
            _dayService = dayService;
            _context = context;
        }

        public TrainingPanelDTO Dto (int dayId) {
            return new TrainingPanelDTO()
            {
                DayId = dayId,
                Day = _dayService.GetDayDateTime(dayId).Date.ToString("dd.MM.yyyy"),
                ClientId = _clientManageService.GetLoggedInClientId(),
                BurnedKcal = _goalsService.CaloriesBurnedInDay(dayId),
                CardioTime = _cardioServices.GetCardioTimeInDay(dayId),
                KcalTarget = _cardioServices.GetKcalBurnedGoalInDay(dayId),
                TimeTarget = _cardioServices.GetTrainingTimeGoalInDay(dayId),
                StrengthTrainings = _context.StrengthTraining.Include(s => s.StrengthTrainingType).Include(s => s.Day).ToList(),
                CardioTrainings = _context.CardioTraining.Include(c => c.CardioTrainingType).Include(c => c.Day).ToList()
            };
        }
    }
}
