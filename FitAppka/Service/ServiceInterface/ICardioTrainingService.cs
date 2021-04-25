using System.Threading.Tasks;
using System.Collections.Generic;
using FitnessApp.Models;
using X.PagedList;
using FitnessApp.Models.DTO;

namespace FitnessApp.Service
{
    public interface ICardioTrainingService
    {
        public void AddCardioTrainingType(int dayID, string name, int kcalPerMin);
        public void AddCardio(int cardioTypeId, int dayID, int timeInMinutes, int burnedKcal);
        public IPagedList<CardioTrainingDTO> GetCardioTrainingsInDay(int dayID, int? page);
        public Task<List<CardioTrainingType>> GetCardioTrainingTypes(string search);
        public bool EditCardio(int id, int time, int burnedKcal);
        public bool DeleteCardio(int id);
        public int? GetKcalBurnedGoalInDay(int dayID);
        public int? GetTrainingTimeGoalInDay(int dayID);
        public int? GetCardioTimeInDay(int dayID);
    }
}
