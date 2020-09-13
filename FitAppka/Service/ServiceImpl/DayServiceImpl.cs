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

        public DayServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository)
        {
            _clientRepository = clientRepository;
            _dayRepository = dayRepository;
        }

        public void AddDayIfNotExists(DateTime day)
        {
            var client = _clientRepository.GetLoggedInClient();
            int count = _dayRepository.GetClientDays(client.ClientId).Count(d => d.Date == day);
            if (count == 0)
            {
                _dayRepository.Add(new Day()
                {
                    Date = day,
                    ClientId = client.ClientId,
                    KcalBurnedGoal = client.KcalBurnedGoal,
                    TrainingTimeGoal = client.TrainingTimeGoal,
                    Breakfast = client.Breakfast,
                    Lunch = client.Lunch,
                    Dinner = client.Dinner,
                    Dessert = client.Dessert,
                    Snack = client.Snack,
                    Supper = client.Supper,
                    ProteinTarget = client.ProteinTarget,
                    FatTarget = client.FatTarget,
                    CarbsTarget = client.CarbsTarget,
                    CalorieGoal = client.CarbsTarget,
                    WaterDrunk = 0
                });
            }
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

        public Day GetClientDayByDate(DateTime date, int clientID)
        {
            return _dayRepository.GetClientDayByDate(date, clientID);
        }
    }
}
