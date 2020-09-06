using System;

namespace FitAppka.Service
{
    public interface ICardioTrainingService
    {
        public int GetKcalBurnedGoalInDay(int dayID);
        public int GetTrainingTimeGoalInDay(int dayID);
        public int CaloriesBurnedInDay(int dayID);
        public int GiveTodayIfDayNotChosen(int dayID);
        public int CardioTimeInDay(int dayID);
        public int GetSelectedDay(DateTime day);
        public int GetClientDayIDByDate(DateTime day);
        public int GetTodayID();
        public void AddDayIfNotExists(DateTime day);
        public void AddCardioTrainingType(string name, int kcalPerMin);
        public void DeleteCardio(int cardioID);
        public void AddCardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal);
    }
}
