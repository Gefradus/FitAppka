using FitnessApp.Models.DTO;

namespace FitnessApp.Service
{
    public interface ITrainingPanelService
    {
        public TrainingPanelDTO Dto(int? dayId);
    }
}
