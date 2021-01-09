using System.Collections.Generic;

namespace FitnessApp.Models.DTO
{
    public class TrainingsDTO
    {
        public List<CardioTrainingType> CardioTrainings { get; set; }
        public List<StrengthTrainingType> StrengthTrainings { get; set; }
    }
}
