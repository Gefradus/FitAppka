using FitAppka.Model;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IStrengthTrainingRepository
    {
        StrengthTraining GetStrengthTraining(int id);
        IEnumerable<StrengthTraining> GetAllStrengthTrainings();
        StrengthTraining Add(StrengthTraining strengthTraining);
        StrengthTraining Update(StrengthTraining strengthTraining);
        StrengthTraining Delete(int id);
    }
}
