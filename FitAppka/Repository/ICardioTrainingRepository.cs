using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface ICardioTrainingRepository
    {
        CardioTraining GetCardioTraining(int id);
        IEnumerable<CardioTraining> GetAllCardioTrainings();
        CardioTraining Add(CardioTraining cardioTraining);
        CardioTraining Update(CardioTraining cardioTraining);
        CardioTraining Delete(int id);
    }
}
