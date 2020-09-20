using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IStrengthTrainingTypeRepository
    {
        StrengthTrainingType GetStrengthTrainingType(int id);
        IEnumerable<StrengthTrainingType> GetAllStrengthTrainingTypes();
        Task<List<StrengthTrainingType>> GetAllStrengthTypesAsync();
        StrengthTrainingType Add(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Update(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Delete(int id);
    }
}
