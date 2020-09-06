using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class CardioTrainingServiceImpl : ICardioTrainingService
    {
        private readonly ICardioTrainingRepository _cardioRepository;
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        

        public CardioTrainingServiceImpl(IDayRepository dayRepository, ICardioTrainingRepository cardioRepository, 
            ICardioTrainingTypeRepository cardioTypeRepository, IClientRepository clientRepository)
        {
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
        }


        public void DeleteCardio(int cardioID)
        {
            if (_dayRepository.GetDay(_cardioRepository.GetCardioTraining(cardioID).DayId).ClientId == _clientRepository.GetLoggedInClient().ClientId)
            {
                _cardioRepository.Delete(cardioID);
            }
        }

        public void AddCardioTrainingType(int dayID, string name, int kcalPerMin)
        {
            if (_clientRepository.GetLoggedInClientId() == _dayRepository.GetDay(dayID).ClientId)
            {
                _cardioTypeRepository.Add(new CardioTrainingType
                {
                    TrainingName = name,
                    KcalPerMin = kcalPerMin,
                    ClientId = _clientRepository.GetLoggedInClient().ClientId
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
            if (timeGoal != null)
            {
                return (int)timeGoal;
            }
            else
            {
                return 0;
            }
        }

        public int CaloriesBurnedInDay(int dayID)
        {
            int? kcal = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID)))
            {
                kcal += cardio.CaloriesBurned;
            }

            return (int)kcal;
        }

        public int GiveTodayIfDayNotChosen(int dayID)
        {
            if (dayID == 0) {
                return GetTodayID();
            }
            else {
                return dayID;
            }
        }

        public int CardioTimeInDay(int dayID)
        {
            int? time = 0;
            foreach (CardioTraining cardio in _cardioRepository.GetAllCardioTrainings().Where(t => t.DayId.Equals(dayID)))
            {
                time += cardio.TimeInMinutes;
            }

            return (int)time;
        }

        public int GetSelectedDay(DateTime day)
        {
            AddDayIfNotExists(day);
            return GetClientDayIDByDate(day);
        }

        public int GetClientDayIDByDate(DateTime day)
        {
            AddDayIfNotExists(day);
            return _dayRepository.GetClientDayByDate(day, _clientRepository.GetLoggedInClient().ClientId).DayId;
        }

        public int GetTodayID()
        {
            return GetSelectedDay(DateTime.Now);
        }

        public void AddDayIfNotExists(DateTime day)
        {
            var client = _clientRepository.GetLoggedInClient();
            int count = _dayRepository.GetAllDays().Count(dz => dz.Date == day && dz.ClientId == client.ClientId);
            if (count == 0)
            {
                _dayRepository.Add(new Day()
                {
                    Date = day,
                    ClientId = client.ClientId,
                    Breakfast = client.Breakfast,
                    Lunch = client.Lunch,
                    Dinner = client.Dinner,
                    Dessert = client.Dessert,
                    Snack = client.Snack,
                    Supper = client.Supper,
                    ProteinTarget = client.ProteinTarget,
                    FatTarget = client.FatTarget,
                    CarbsTarget = client.CarbsTarget,
                    CalorieTarget = client.CarbsTarget,
                    WaterDrunk = 0,
                });
            }
        }


    }
}
