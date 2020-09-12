using System;
namespace FitAppka.Service
{
    public interface IDayService
    {
        public void AddDayIfNotExists(DateTime day);
        public int GetDayIDByDate(DateTime day);
        public int GetTodayID();
        public DateTime GetDayDateTime(int id);
    }
}
