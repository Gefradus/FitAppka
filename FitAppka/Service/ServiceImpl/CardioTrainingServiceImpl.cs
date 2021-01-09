using FitnessApp.Models;
using FitnessApp.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Service.ServiceImpl
{
    public class CardioTrainingServiceImpl : ICardioTrainingService
    {
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IGoalsService _goalsService;
        private readonly IClientManageService _clientManageService;
        
        public CardioTrainingServiceImpl(ICardioTrainingRepository cardioRepository, IClientManageService clientManageService,
            IGoalsService goalsService, ICardioTrainingTypeRepository cardioTypeRepository, IGoalsRepository goalsRepository)
        {
            _goalsRepository = goalsRepository;
            _goalsService = goalsService;
            _clientManageService = clientManageService;
            _cardioTypeRepository = cardioTypeRepository;
            _cardioRepository = cardioRepository;
        }

        public void AddCardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal)
        {
            _cardioRepository.Add(new CardioTraining()
            {
                DayId = dayID,
                TimeInMinutes = timeInMinutes,
                CardioTrainingTypeId = cardioTypeId,
                CaloriesBurned = burnedKcal
            });


            _goalsService.UpdateGoalsInLoggedInClientDaysFromToday();
        }

        public bool EditCardio(int id, int time, int burnedKcal)
        {
            CardioTraining cardio = _cardioRepository.GetCardioTraining(id);
            if (_clientManageService.HasUserAccessToDay(cardio.DayId))
            {
                cardio.TimeInMinutes = time;
                cardio.CaloriesBurned = burnedKcal;
                _cardioRepository.Update(cardio);
                _goalsService.UpdateGoalsInLoggedInClientDaysFromToday();
                return true;
            }
            return false;
        }

        public bool DeleteCardio(int id)
        {
            if(_clientManageService.HasUserAccessToDay(_cardioRepository.GetCardioTraining(id).DayId)){
                _cardioRepository.Delete(id);
                _goalsService.UpdateGoalsInLoggedInClientDaysFromToday();
                return true;
            }
            return false;
        }

        public void AddCardioTrainingType(int dayID, string name, int kcalPerMin)
        {
            if (_clientManageService.HasUserAccessToDay(dayID))
            {
                _cardioTypeRepository.Add(new CardioTrainingType
                {
                    TrainingName = name,
                    KcalPerMin = kcalPerMin,
                    ClientId = _clientManageService.GetLoggedInClientId(),
                    VisibleToAll = _clientManageService.IsLoggedInClientAdmin(),
                    IsDeleted = false
                });
            }
        }

        

        public int? GetKcalBurnedGoalInDay(int dayID)
        {
            int? kcalBurnedGoal = _goalsRepository.GetDayGoals(dayID).KcalBurned;
            if (kcalBurnedGoal != null)
            {
                return kcalBurnedGoal;
            }
            else
            {
                return 0;
            }
        }

        public int? GetTrainingTimeGoalInDay(int dayID)
        {
            int? timeGoal = _goalsRepository.GetDayGoals(dayID).TrainingTime;
            if (timeGoal != null) {
                return timeGoal;
            }
            else {
                return 0;
            }
        }

        public int? GetCardioTimeInDay(int dayID)
        {
            int? time = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID))) {
                time += cardio.TimeInMinutes;
            }

            return time;
        }

        public Task<List<CardioTrainingType>> GetCardioTrainingTypes(string search)
        {
            return _cardioTypeRepository.GetAllCardioTypesAsync(search);
        }

        
    }
}
