using System;

namespace FitAppka.Service
{
    public interface ICardioTrainingService
    {
        public int GetKcalBurnedGoalInDay(int dayID);
        public int GetTrainingTimeGoalInDay(int dayID);
        public int CaloriesBurnedInDay(int dayID);
        public int CardioTimeInDay(int dayID);
        public void AddCardioTrainingType(int dayID, string name, int kcalPerMin);
        public void AddCardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal);
        public bool EditCardio(int id, int time, int burnedKcal);
        public bool DeleteCardio(int id);
    }
}
