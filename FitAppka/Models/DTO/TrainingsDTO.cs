using System.Collections.Generic;

namespace FitAppka.Models.DTO
{
    public class TrainingsDTO
    {
        public List<CardioTrainingType> CardioTrainings { get; set; }
        public List<StrengthTrainingType> StrengthTrainings { get; set; }
    }
}
