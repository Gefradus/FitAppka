using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface ICardioTrainingTypeRepository
    {
        CardioTrainingType GetCardioTrainingType(int id);
        IEnumerable<CardioTrainingType> GetAllCardioTrainingTypes();
        CardioTrainingType Add(CardioTrainingType cardioType);
        CardioTrainingType Update(CardioTrainingType cardioType);
        CardioTrainingType Delete(int id);
    }
}
