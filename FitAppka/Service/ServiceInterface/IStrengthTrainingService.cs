using System.Collections.Generic;
using System.Threading.Tasks;
namespace FitAppka.Service
{
    public interface IStrengthTrainingService
    {
        public void AddStrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight);
        public bool EditStrengthTraining(int id, short sets, short reps, short weight);
        public Task<List<Models.StrengthTrainingType>> GetStrengthTrainingTypes(string search);
        public bool DeleteStrengthTraining(int id);
        public void AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight);
        
    }
}
