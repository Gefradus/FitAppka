using FitnessApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Repository
{
    public interface ICardioTrainingTypeRepository
    {
        CardioTrainingType GetCardioType(int id);
        IEnumerable<CardioTrainingType> GetAllCardioTypes(string searchCardio);
        Task<List<CardioTrainingType>> GetAllCardioTypesAsync(string search);
        CardioTrainingType Add(CardioTrainingType cardioType);
        CardioTrainingType Update(CardioTrainingType cardioType);
        CardioTrainingType Delete(int id);
    }
}
