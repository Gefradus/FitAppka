using System.Collections.Generic;
using System.Threading.Tasks;

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
        public List<CardioTraining> CardioTrainings { get; set; }
        public List<StrengthTraining> StrengthTrainings { get; set; }
    }
}
