using System;

namespace FitAppka.Service
{
    public interface IStrengthTrainingService
    {
        public void AddStrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight);
        public void AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight);

    }
}
