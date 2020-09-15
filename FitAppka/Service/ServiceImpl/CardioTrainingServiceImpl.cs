using FitAppka.Model;
using FitAppka.Repository;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class CardioTrainingServiceImpl : ICardioTrainingService
    {
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly IDietaryTargetsService _dietaryTargetsService;
        private readonly IClientManageService _clientManageService;
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;

        public CardioTrainingServiceImpl(IDayRepository dayRepository, ICardioTrainingRepository cardioRepository, IDietaryTargetsService dietaryTargetsService,
            ICardioTrainingTypeRepository cardioTypeRepository, IClientRepository clientRepository, IClientManageService clientManageService)
        {
            _dietaryTargetsService = dietaryTargetsService;
            _clientManageService = clientManageService;
            _cardioTypeRepository = cardioTypeRepository;
            _clientRepository = clientRepository;
            _cardioRepository = cardioRepository;
            _dayRepository = dayRepository;
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


            _dietaryTargetsService.UpdateTargetsInDaysFromToday(_clientRepository.GetLoggedInClient());
        }

        public bool EditCardio(int id, int time, int burnedKcal)
        {
            CardioTraining cardio = _cardioRepository.GetCardioTraining(id);
            if (_clientManageService.HasUserAccessToDay(cardio.DayId))
            {
                cardio.TimeInMinutes = time;
                cardio.CaloriesBurned = burnedKcal;
                _cardioRepository.Update(cardio);
                _dietaryTargetsService.UpdateTargetsInDaysFromToday(_clientRepository.GetLoggedInClient());
                return true;
            }
            return false;
        }

        public bool DeleteCardio(int id)
        {
            if(_clientManageService.HasUserAccessToDay(_cardioRepository.GetCardioTraining(id).DayId)){
                _cardioRepository.Delete(id);
                _dietaryTargetsService.UpdateTargetsInDaysFromToday(_clientRepository.GetLoggedInClient());
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
                    ClientId = _clientRepository.GetLoggedInClientId(),
                    VisibleToAll = _clientRepository.IsLoggedInClientAdmin()
                });
            }
        }

        public int GetKcalBurnedGoalInDay(int dayID)
        {
            int? kcalBurnedGoal = _dayRepository.GetDay(dayID).KcalBurnedGoal;
            if (kcalBurnedGoal != null)
            {
                return (int)kcalBurnedGoal;
            }
            else
            {
                return 0;
            }
        }

        public int GetTrainingTimeGoalInDay(int dayID)
        {
            int? timeGoal = _dayRepository.GetDay(dayID).TrainingTimeGoal;
            if (timeGoal != null) {
                return (int)timeGoal;
            }
            else {
                return 0;
            }
        }

        public int CardioTimeInDay(int dayID)
        {
            int? time = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID))) {
                time += cardio.TimeInMinutes;
            }

            return (int)time;
        }  
    }
}
