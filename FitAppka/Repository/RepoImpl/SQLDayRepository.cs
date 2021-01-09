using FitnessApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Repository.RepIfaceImpl
{
    public class SQLDayRepository : IDayRepository
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;

        public SQLDayRepository(FitAppContext context, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public Day Add(Day day)
        {
            _context.Add(day);
            _context.SaveChanges();
            return day;
        }

        public Day Delete(int id)
        {
            Day day = GetDay(id);

            if (day != null)
            {
                _context.Day.Remove(day);
                _context.SaveChanges();
            }

            return day;
        }

        public IEnumerable<Day> GetAllDays()
        {
            return _context.Day.ToList();
        }

        public Day GetClientDayByDate(DateTime date, int clientID)
        {
            return _context.Day.FirstOrDefault(d => d.Date == date && d.ClientId == clientID);
        }

        public IEnumerable<Day> GetLoggedInClientDays()
        {
            return GetClientDays(_clientRepository.GetLoggedInClientId());
        }

        public IEnumerable<Day> GetClientDays(int clientId)
        {
            return _context.Day.Where(d => d.ClientId == clientId).ToList();
        }

        public Day GetDay(int id)
        {
            return _context.Day.Find(id);
        }

        public DateTime GetDayDateTime(int id)
        {
            return (DateTime) GetDay(id).Date;
        }

        public Day Update(Day day)
        {
            _context.Update(day);
            _context.SaveChanges();
            return day;
        }

        
    }
}
