using FitnessApp.Models;
using System;
using System.Collections.Generic;

namespace FitnessApp.Repository
{
    public interface IDayRepository
    {
        Day GetDay(int id);
        IEnumerable<Day> GetAllDays();
        IEnumerable<Day> GetLoggedInClientDays();
        IEnumerable<Day> GetClientDays(int clientId);
        Day Add(Day day);
        Day Update(Day day);
        Day Delete(int id);
        DateTime GetDayDateTime(int id);
        Day GetClientDayByDate(DateTime date, int clientID);
    }
}
