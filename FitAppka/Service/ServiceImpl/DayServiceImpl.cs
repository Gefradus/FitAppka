using AutoMapper;
using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class DayServiceImpl : IDayManageService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IGoalsRepository _goalsRepository;
        private readonly IMapper _mapper;

        public DayServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository, 
            IGoalsRepository goalsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _goalsRepository = goalsRepository;
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public void AddDayIfNotExists(DateTime date)
        {
            var client = _clientRepository.GetLoggedInClient();
            int count = _dayRepository.GetClientDays(client.ClientId).Count(d => d.Date == date);
            if (count == 0)
            {
                Goals goals = MapGoalsFromClientToDay(client.ClientId);
                Day day = _dayRepository.Add(new Day()
                {
                    Goals = goals,
                    Date = date,
                    ClientId = client.ClientId,
                    Breakfast = client.Breakfast,
                    Lunch = client.Lunch,
                    Dinner = client.Dinner,
                    Dessert = client.Dessert,
                    Snack = client.Snack,
                    Supper = client.Supper,
                    WaterDrunk = 0
                });

                goals.DayId = day.DayId;
                _goalsRepository.Update(goals);
            }
        }

        private Goals MapGoalsFromClientToDay(int clientId)
        {
            Goals clientGoals = _goalsRepository.GetClientGoals(clientId);
            return new Goals()
            {
                Calories = clientGoals.Calories,
                Proteins = clientGoals.Proteins,
                Carbohydrates = clientGoals.Carbohydrates,
                Fats = clientGoals.Fats,
                KcalBurned = clientGoals.KcalBurned,
                TrainingTime = clientGoals.TrainingTime,
                VitaminA = clientGoals.VitaminA,
                VitaminB1 = clientGoals.VitaminB1,
                VitaminB2 = clientGoals.VitaminB2,
                VitaminB5 = clientGoals.VitaminB5,
                VitaminB6 = clientGoals.VitaminB6,
                VitaminC = clientGoals.VitaminC,
                VitaminD = clientGoals.VitaminD,
                VitaminE = clientGoals.VitaminE,
                VitaminK = clientGoals.VitaminK,
                VitaminPp = clientGoals.VitaminPp,
                Biotin = clientGoals.Biotin,
                FolicAcid = clientGoals.FolicAcid,
                VitaminB12 = clientGoals.VitaminB12,
                Zinc = clientGoals.Zinc,
                Phosphorus = clientGoals.Phosphorus,
                Iodine = clientGoals.Iodine,
                Magnesium = clientGoals.Magnesium,
                Copper = clientGoals.Copper,
                Potassium = clientGoals.Potassium,
                Selenium = clientGoals.Selenium,
                Sodium = clientGoals.Sodium,
                Calcium = clientGoals.Calcium,
                Iron = clientGoals.Iron
            };
        }


        public int GetDayIDByDate(DateTime day)
        {
            AddDayIfNotExists(day);
            return _dayRepository.GetClientDayByDate(day, _clientRepository.GetLoggedInClientId()).DayId;
        }

        public int GetTodayId()
        {
            return GetDayIDByDate(DateTime.Now.Date);
        }

        public DateTime GetDayDateTime(int id)
        {
            return _dayRepository.GetDayDateTime(id);
        }

        public IEnumerable<Day> GetAllDays()
        {
            return _dayRepository.GetAllDays();
        }

        public Day GetLoggedInClientDayByDate(DateTime date)
        {
            return _dayRepository.GetClientDayByDate(date, _clientRepository.GetLoggedInClientId());
        }

        public Goals GetDayGoals(int dayId)
        {
            return _goalsRepository.GetDayGoals(dayId);
        }
    }
}
