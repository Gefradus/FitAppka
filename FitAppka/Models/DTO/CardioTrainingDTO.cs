namespace FitnessApp.Models.DTO
{
    public class CardioTrainingDTO
    {
        public int Id { get; set; }
        public int TimeInMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public string TrainingName { get; set; }
        public int KcalPerMin { get; set; }
    }
}
