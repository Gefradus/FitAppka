using FitAppka.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLGoalsRepositoryImpl : IGoalsRepository
    {
        private readonly FitAppContext _context;

        public SQLGoalsRepositoryImpl(FitAppContext context)
        {
            _context = context;
        }

        public Goals Add(Goals goals)
        {
            _context.Add(goals);
            _context.SaveChanges();
            return goals;
        }

        public Goals Delete(int id)
        {
            Goals goals = GetGoals(id);

            if (goals != null)
            {
                _context.Goals.Remove(goals);
                _context.SaveChanges();
            }

            return goals;
        }

        public IEnumerable<Goals> GetAllClientsGoals()
        {
            return _context.Goals.Where(g => g.ClientId != null).ToList();
        }

        public IEnumerable<Goals> GetAllDaysGoals()
        {
            return _context.Goals.Where(g => g.DayId != null).ToList();
        }

        public IEnumerable<Goals> GetAllGoals()
        {
            return _context.Goals.ToList();
        }

        public Goals GetClientGoals(int clientId)
        {
            return _context.Goals.FirstOrDefault(c => c.ClientId == clientId);
        }

        public Goals GetClientGoalsAsNoTracking(int clientId)
        {
            return _context.Goals.AsNoTracking().FirstOrDefault(c => c.ClientId == clientId);
        }

        public Goals GetDayGoals(int dayId)
        {
            return _context.Goals.FirstOrDefault(d => d.DayId == dayId);
        }

        public Goals GetGoals(int id)
        {
            return _context.Goals.Find(id);
        }

        public Goals Update(Goals goals)
        {
            _context.Update(goals);
            _context.SaveChanges();
            return goals;
        }
    }
}
