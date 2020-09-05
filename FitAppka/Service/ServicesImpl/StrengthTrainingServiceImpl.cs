using FitAppka.Models;
using FitAppka.Repository;

namespace FitAppka.Services.ServicesImpl
{
    public class StrengthTrainingServiceImpl : IStrengthTrainingService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IStrengthTrainingRepository _strengthTrainingRepository;
        private readonly IStrengthTrainingTypeRepository _strengthTrainingTypeRepository;

        public StrengthTrainingServiceImpl(IStrengthTrainingRepository strengthTrainingRepository, IStrengthTrainingTypeRepository strengthTrainingTypeRepository)
        {
            _strengthTrainingTypeRepository = strengthTrainingTypeRepository;
            _strengthTrainingRepository = strengthTrainingRepository;
        }

        public void AddStrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight)
        {
            _strengthTrainingRepository.Add(new StrengthTraining()
            {
                StrengthTrainingTypeId = trainingTypeId,
                DayId = dayID,
                Sets = sets,
                Repetitions = reps,
                Weight = weight
            });
        }

        public void AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight)
        {
            StrengthTrainingType type = _strengthTrainingTypeRepository.Add(new StrengthTrainingType()
            {
                VisibleToAll = false,
                ClientId = _clientRepository.GetLoggedInClient().ClientId,
                TrainingName = name
            });

            _strengthTrainingRepository.Add(new StrengthTraining()
            {
                StrengthTrainingTypeId = type.StrengthTrainingTypeId,
                DayId = dayID,
                Sets = sets,
                Repetitions = reps,
                Weight = weight
            });
        }
    }
}
