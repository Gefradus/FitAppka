using FitAppka.Models;
using FitAppka.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Service.ServiceImpl
{
    public class StrengthTrainingServiceImpl : IStrengthTrainingService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientManageService _clientManageService;
        private readonly IStrengthTrainingRepository _strengthTrainingRepository;
        private readonly IStrengthTrainingTypeRepository _strengthTrainingTypeRepository;

        public StrengthTrainingServiceImpl(IStrengthTrainingRepository strengthTrainingRepository, IClientManageService clientManageService,
            IStrengthTrainingTypeRepository strengthTrainingTypeRepository, IClientRepository clientRepository)
        {
            _clientManageService = clientManageService;
            _clientRepository = clientRepository;
            _strengthTrainingTypeRepository = strengthTrainingTypeRepository;
            _strengthTrainingRepository = strengthTrainingRepository;
        }

        public void AddStrengthTraining(int trainingTypeId, int dayID, short sets, short reps, short weight)
        {
            if (_clientManageService.HasUserAccessToDay(dayID))
            {
                _strengthTrainingRepository.Add(new StrengthTraining()
                {
                    StrengthTrainingTypeId = trainingTypeId,
                    DayId = dayID,
                    Sets = sets,
                    Repetitions = reps,
                    Load = weight
                });
            }
        }

        public void AddStrengthTrainingType(int dayID, string name, short sets, short reps, short weight)
        {
            if (_clientManageService.HasUserAccessToDay(dayID))
            {
                StrengthTrainingType type = _strengthTrainingTypeRepository.Add(new StrengthTrainingType()
                {
                    VisibleToAll = _clientRepository.IsLoggedInClientAdmin(),
                    ClientId = _clientRepository.GetLoggedInClientId(),
                    TrainingName = name,
                    IsDeleted = false
                });

                _strengthTrainingRepository.Add(new StrengthTraining()
                {
                    StrengthTrainingTypeId = type.StrengthTrainingTypeId,
                    DayId = dayID,
                    Sets = sets,
                    Repetitions = reps,
                    Load = weight
                });
            }
        }

        public bool DeleteStrengthTraining(int id)
        {
            if (_clientManageService.HasUserAccessToDay(_strengthTrainingRepository.GetStrengthTraining(id).DayId))
            {
                _strengthTrainingRepository.Delete(id);
                return true;
            }
            return false;
        }

        public bool EditStrengthTraining(int id, short sets, short reps, short weight)
        {
            StrengthTraining training = _strengthTrainingRepository.GetStrengthTraining(id);
            if (_clientManageService.HasUserAccessToDay(training.DayId))
            {
                training.Sets = sets;
                training.Repetitions = reps;
                training.Load = weight;
                _strengthTrainingRepository.Update(training);
                return true;
            }
            return false;
        }

        public Task<List<StrengthTrainingType>> GetStrengthTrainingTypes(string search)
        {
            return _strengthTrainingTypeRepository.GetAllStrengthTypesAsync(search);
        }
    }
}
