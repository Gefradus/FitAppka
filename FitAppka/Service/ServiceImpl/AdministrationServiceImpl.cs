using FitAppka.Service.ServiceInterface;
using FitAppka.Repository;
using FitAppka.Models;
using AutoMapper;

namespace FitAppka.Service.ServiceImpl
{
    public class AdministrationServiceImpl : IAdministrationService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;

        public AdministrationServiceImpl(IClientRepository clientRepository, IMapper mapper,
            IWeightMeasurementRepository weightMeasurementRepository, ISettingsService settingsService)
        {
            _settingsService = settingsService;
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
            _settingsService.ChangeSettings(dto.SettingsDTO, 0, id);
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
    }
}
