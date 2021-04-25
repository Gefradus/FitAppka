using FitnessApp.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace FitnessApp.Service
{
    public interface IStrengthTrainingService
    {
        void AddStrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight);
        bool EditStrengthTraining(int id, short sets, short reps, short weight);
        IPagedList<StrengthTrainingDTO> GetStrengthTrainingsInDay(int dayID, int? page);
        Task<List<Models.StrengthTrainingType>> GetStrengthTrainingTypes(string search);
        bool DeleteStrengthTraining(int id);
        void AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight);
        
    }
}
