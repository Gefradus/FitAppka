using X.PagedList;

namespace FitnessApp.Models.DTO
{
    public class TrainingPanelDTO
    {
        public int DayId { get; set; }
        public string Day { get; set; }
        public int ClientId { get; set; }
        public int BurnedKcal { get; set; }
        public int? CardioTime { get; set; }
        public int? KcalTarget { get; set; }
        public int? TimeTarget { get; set; }
        public IPagedList<CardioTrainingDTO> CardioTrainings { get; set; }
        public IPagedList<StrengthTrainingDTO> StrengthTrainings { get; set; }
        public int? CardioTrainingsPage { get; set; }
        public int? StrengthTrainingsPage { get; set; }
    }
}
