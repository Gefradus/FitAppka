using FitnessApp.Models;
using System.Collections.Generic;

namespace FitnessApp.Repository
{
    public interface IGoalsRepository
    {
        Goals GetGoals(int id);
        IEnumerable<Goals> GetAllGoals();
        IEnumerable<Goals> GetAllClientsGoals();
        IEnumerable<Goals> GetAllDaysGoals();
        Goals GetClientGoals(int clientId);
        Goals GetClientGoalsAsNoTracking(int clientId);
        Goals GetDayGoals(int dayId);
        Goals Add(Goals goals);
        Goals Update(Goals goals);
        Goals Delete(int id);
    }
}

