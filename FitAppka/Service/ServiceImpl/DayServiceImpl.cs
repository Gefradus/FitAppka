using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Service.ServiceImpl
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
            int count = _dayRepository.GetLoggedInClientDays().Count(d => d.Date == date);
            if (count == 0)
            {
                Client client = _clientRepository.GetLoggedInClientAsNoTracking();
                Goals goals = _goalsRepository.Add(_mapper.Map<Goals, Goals>(_goalsRepository.GetClientGoalsAsNoTracking(client.ClientId)));

                Day day = _mapper.Map<Client, Day>(_clientRepository.GetLoggedInClientAsNoTracking());
                day.Date = date;
                day.GoalsId = goals.GoalsId;
                _dayRepository.Add(day);

                goals.DayId = day.DayId;
                _goalsRepository.Update(goals);
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
