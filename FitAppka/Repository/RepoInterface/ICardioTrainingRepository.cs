using FitnessApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Repository
{
    public interface ICardioTrainingRepository
    {
        CardioTraining GetCardioTraining(int id);
        IEnumerable<CardioTraining> GetAllCardioTrainings();
        Task<List<CardioTraining>> GetAllCardioTrainingsAsync();
        CardioTraining Add(CardioTraining cardioTraining);
        CardioTraining Update(CardioTraining cardioTraining);
        CardioTraining Delete(int id);
    }
}
