using FitAppka.Models;
using System;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IDayRepository
    {
        Day GetDay(int id);
        IEnumerable<Day> GetAllDays();
        IEnumerable<Day> GetClientDays(int clientID);
        Day Add(Day day);
        Day Update(Day day);
        Day Delete(int id);
        DateTime GetDayDateTime(int id);
        Day GetClientDayByDate(DateTime date, int clientID);
    }
}
