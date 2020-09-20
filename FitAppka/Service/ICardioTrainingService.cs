namespace FitAppka.Service
{
    public interface ICardioTrainingService
    {
        public void AddCardioTrainingType(int dayID, string name, int kcalPerMin);
        public void AddCardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal);
        public bool EditCardio(int id, int time, int burnedKcal);
        public bool DeleteCardio(int id);
        public int? GetKcalBurnedGoalInDay(int dayID);
        public int? GetTrainingTimeGoalInDay(int dayID);
        public int? GetCardioTimeInDay(int dayID);
    }
}
