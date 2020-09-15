using FitAppka.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface ICardioTrainingTypeRepository
    {
        CardioTrainingType GetCardioType(int id);
        IEnumerable<CardioTrainingType> GetAllCardioTypes();
        Task<List<CardioTrainingType>> GetAllCardioTypesAsync();
        CardioTrainingType Add(CardioTrainingType cardioType);
        CardioTrainingType Update(CardioTrainingType cardioType);
        CardioTrainingType Delete(int id);
    }
}
