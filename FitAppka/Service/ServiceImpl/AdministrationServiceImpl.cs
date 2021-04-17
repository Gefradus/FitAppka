using FitnessApp.Service.ServiceInterface;
using FitnessApp.Repository;
using FitnessApp.Models;
using AutoMapper;
using FitnessApp.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using FitnessApp.Repository.RepoInterface;
using FitnessApp.Models.DTO.DietCreatorDTO;

namespace FitnessApp.Service.ServiceImpl
{
    public class AdministrationServiceImpl : IAdministrationService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDietRepository _dietRepository;
        private readonly IDietProductRepository _dietProductRepository;
        private readonly ICardioTrainingTypeRepository _cardioTypeRepository;
        private readonly IStrengthTrainingTypeRepository _strengthTrainingTypeRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly ISettingsService _settingsService;
        private readonly IDietCreatorService _dietService;
        private readonly IMapper _mapper;

        public AdministrationServiceImpl(IClientRepository clientRepository, IMapper mapper, IDietRepository dietRepository, 
            IDietCreatorService dietCreatorService, ICardioTrainingTypeRepository cardioRepository, 
            IWeightMeasurementRepository weightMeasurementRepository, ISettingsService settingsService, 
            IStrengthTrainingTypeRepository strengthTrainingRepository, IDietProductRepository dietProductRepository)
        {
            _dietService = dietCreatorService;
            _dietProductRepository = dietProductRepository;
            _dietRepository = dietRepository;
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
                _settingsService.ChangeSettings(dto.SettingsDTO, false, id);
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

        public bool EditStrengthTrainingType(int id, string name, bool visibleToAll)
        {
            if (_clientRepository.IsLoggedInClientAdmin())
            {
                var type = _strengthTrainingTypeRepository.GetStrengthTrainingType(id);
                type.TrainingName = name;
                type.VisibleToAll = visibleToAll;
                _strengthTrainingTypeRepository.Update(type);
                return true;
            }
            return false;
        }

        public List<AdminDietDTO> GetAdminDiets()
        {
            var list = new List<DietDTO>();
            foreach (var item in _dietRepository.GetAllDiets().Where(d => d.IsDeleted == false))
            {
                list.Add(_mapper.Map<Diet, DietDTO>(item));
            }

            var listOfAdminDiets = new List<AdminDietDTO>();
            foreach (var item in list)
            {
                var dietProducts = _dietProductRepository.GetDietProducts(item.DietId);
                listOfAdminDiets.Add(new AdminDietDTO()
                {
                    Diets = new ActiveDietDTO()
                    {
                        Diet = item,
                        Products = _dietService.MapProductsToDietProductsDTO(_dietService.MapDietProductsToDTO(dietProducts)),
                        CaloriesSum = _dietService.CountCaloriesSum(dietProducts)
                    },
                    ClientId = _dietRepository.GetDiet(item.DietId).ClientId
                });   
            }
            return SortListOfActiveDiets(listOfAdminDiets);
        }


        public List<AdminDietDTO> SortListOfActiveDiets(List<AdminDietDTO> list)
        {
            return list.OrderBy(l => !l.Diets.Diet.Active).ThenBy(l => !l.Diets.Diet.Monday).ThenBy(l => !l.Diets.Diet.Tuesday).ThenBy(l => !l.Diets.Diet.Wednesday)
                .ThenBy(l => !l.Diets.Diet.Thursday).ThenBy(l => !l.Diets.Diet.Friday).ThenBy(l => !l.Diets.Diet.Saturday).ThenBy(l => !l.Diets.Diet.Sunday)
                .ThenBy(l => l.Diets.CaloriesSum).ToList();
        }

    }
}
