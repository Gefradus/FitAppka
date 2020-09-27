using FitAppka.Models;
using System;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IDayRepository
    {
        Day GetDay(int id);
        IEnumerable<Day> GetAllDays();
        IEnumerable<Day> GetLoggedInClientDays();
        Day Add(Day day);
        Day Update(Day day);
        Day Delete(int id);
        DateTime GetDayDateTime(int id);
        Day GetClientDayByDate(DateTime date, int clientID);
    }
}
