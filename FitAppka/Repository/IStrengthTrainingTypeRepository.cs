using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IStrengthTrainingTypeRepository
    {
        StrengthTrainingType GetStrengthTrainingType(int id);
        IEnumerable<StrengthTrainingType> GetAllStrengthTrainingTypes();
        StrengthTrainingType Add(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Update(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Delete(int id);
    }
}
