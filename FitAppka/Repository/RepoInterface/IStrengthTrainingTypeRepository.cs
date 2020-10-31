using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IStrengthTrainingTypeRepository
    {
        StrengthTrainingType GetStrengthTrainingType(int id);
        IEnumerable<StrengthTrainingType> GetAllStrengthTrainingTypes(string search);
        Task<List<StrengthTrainingType>> GetAllStrengthTypesAsync(string search);
        StrengthTrainingType Add(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Update(StrengthTrainingType strengthTrainingType);
        StrengthTrainingType Delete(int id);
    }
}
