using FitAppka.Service.ServiceInterface;
using FitAppka.Repository;
using FitAppka.Models;
using AutoMapper;
using FitAppka.Models.DTO;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class AdministrationServiceImpl : IAdministrationService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly IStrengthTrainingTypeRepository _strengthTrainingTypeRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;

        

        public AdministrationServiceImpl(IClientRepository clientRepository, IMapper mapper,
            ICardioTrainingTypeRepository cardioRepository, IWeightMeasurementRepository weightMeasurementRepository, 
            ISettingsService settingsService, IStrengthTrainingTypeRepository strengthTrainingRepository)
        {
            _settingsService = settingsService;
            _strengthTrainingTypeRepository = strengthTrainingRepository;
            _cardioTypeRepository = cardioRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _weightMeasurementRepository = weightMeasurementRepository;
        }

        public bool BanClient(int id)
        {
            if(_clientRepository.GetLoggedInClientId() != id) {
                _clientRepository.BanClient(id);
                return true;
            }
            return false;
        }

        public void EditClient(ClientAdministrationDTO dto, int id)
        {
            try {
                _settingsService.ChangeSettings(dto.SettingsDTO, 0, id);
            }
            catch {}
            EditRegisterDTO(dto.RegisterDTO, id);
        }

        private void EditRegisterDTO(RegisterDTO dto, int id) {
            Client c = _clientRepository.GetClientById(id);
            c.FirstName = dto.FirstName;
            c.SecondName = dto.SecondName;
            c.Login = dto.Login;
            c.Email = dto.Email;
            _clientRepository.Update(c);
        }


        public ClientAdministrationDTO GetClientAdministrationDTO(int id)
        {
            Client client = _clientRepository.GetClientById(id);
            SettingsDTO settingsDTO = _mapper.Map<Client, SettingsDTO>(client);
            settingsDTO.Weight = _weightMeasurementRepository.GetLastClientWeight(id);
            RegisterDTO registerDTO = _mapper.Map<Client, RegisterDTO>(client);
            return new ClientAdministrationDTO() {
                RegisterDTO = registerDTO,
                SettingsDTO = settingsDTO
            };
        }

        public bool UnbanClient(int id)
        {
            if (_clientRepository.GetLoggedInClientId() != id) {
                _clientRepository.UnbanClient(id);
                return true;
            }
            return false;
        }

        public TrainingsDTO GetTrainingsDTO(string searchCardio, string searchStrength)
        {
            return new TrainingsDTO()
            {
                CardioTrainings = _cardioTypeRepository.GetAllCardioTypes(searchCardio).ToList(),
                StrengthTrainings = _strengthTrainingTypeRepository.GetAllStrengthTrainingTypes(searchStrength).ToList()
            };
        }

        public bool AddCardioType(string name, int kcalPerMin, bool visibleToAll)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _cardioTypeRepository.Add(new CardioTrainingType()
                {
                    TrainingName = name,
                    KcalPerMin = kcalPerMin,
                    ClientId = _clientRepository.GetLoggedInClientId(),
                    VisibleToAll = visibleToAll,
                    IsDeleted = false
                });
                return true;
            }
            return false;
        }

        public bool DeleteCardioType(int id)
        {
            if (_clientRepository.IsLoggedInClientAdmin()) {
                _cardioTypeRepository.Delete(id);
                return true;
            }
            return false;
        }

        public bool DeleteStrengthTrainingType(int id)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _strengthTrainingTypeRepository.Delete(id);
                return true;
            }
            return false;
        }

        public bool AddStrengthTrainingType(string name, bool visibleToAll)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                _strengthTrainingTypeRepository.Add(new StrengthTrainingType()
                {
                    ClientId = _clientRepository.GetLoggedInClientId(),
                    VisibleToAll = visibleToAll,
                    TrainingName = name,
                    IsDeleted = false
                });
                return true;
            }
            return false;
        }

        public bool EditCardioType(int id, string name, int kcalPerMin, bool visibleToAll)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                var type = _cardioTypeRepository.GetCardioType(id);
                type.TrainingName = name;
                type.KcalPerMin = kcalPerMin;
                type.VisibleToAll = visibleToAll;
                _cardioTypeRepository.Update(type);
                return true;
            }
            return false;
        }
    }
}
