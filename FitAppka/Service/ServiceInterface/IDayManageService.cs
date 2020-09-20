using FitAppka.Models;
using System;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IDayManageService
    {
        public void AddDayIfNotExists(DateTime day);
        public int GetDayIDByDate(DateTime day);
        public int GetTodayId();
        public IEnumerable<Day> GetAllDays();
        public DateTime GetDayDateTime(int id);
        public Day GetLoggedInClientDayByDate(DateTime date);
        Goals GetDayGoals(int dayId);
    }
}
