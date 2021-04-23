using FitnessApp.Models;
using FitnessApp.Models.DTO;
using FitnessApp.Repository;
using FitnessApp.Repository.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Service.ServiceImpl
{
    public class ClientManageServiceImpl : IClientManageService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;
        private readonly IDietRepository _dietRepository;

        public ClientManageServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository, IDietRepository dietRepository,
            IProductRepository productRepository, IWeightMeasurementRepository weightMeasurementRepository)
        {
            _dietRepository = dietRepository;
            _weightMeasurementRepository = weightMeasurementRepository;
            _productRepository = productRepository;
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
        }

        public async Task AddNewClient(RegisterDTO model)
        {
            await _clientRepository.AddAsync(new Client()
            {
                Login = model.Login,
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                IsAdmin = false,
                AutoDietaryGoals = true,
                IncludeCaloriesBurned = false,
                IsBanned = false,
                DateOfJoining = DateTime.Now
            });
        }

        private Client GetClientFromModel(LoginDTO model)
        {
            string loginOrEmail = model.LoginOrEmail.ToLower();
            return _clientRepository.GetAllClients().FirstOrDefault(c => c.Login.ToLower().Equals(loginOrEmail) || c.Email.ToLower().Equals(loginOrEmail));
        }

        public string GetClientLoginFromModel(LoginDTO model)
        {
            return GetClientFromModel(model).Login;
        }

        public bool CheckIfClientFromModelIsBanned(LoginDTO model)
        {
            return GetClientFromModel(model).IsBanned;
        }

        public bool CheckIfPassCorrect(LoginDTO model)
        {
            Client client = GetClientFromModel(model);
            return client != null && client.Password.Equals(model.Password);
        }

        public bool HasUserAccessToDay(int dayID)
        {
            return HasUserAccess(_dayRepository.GetDay(dayID).ClientId);
        }

        public bool HasUserAccessToProduct(int productID)
        {
            return HasUserAccess(_productRepository.GetProductAsNoTracking(productID).ClientId);
        }

        public bool HasUserAccessToWeightMeasurement(int id)
        {
            return HasUserAccess(_weightMeasurementRepository.GetWeightMeasurement(id).ClientId);
        }

        public bool HasUserAccessToDiet(int id)
        {
            return HasUserAccess(_dietRepository.GetDiet(id).ClientId);
        }

        private bool HasUserAccess(int clientID)
        {
            return clientID == _clientRepository.GetLoggedInClientId() || _clientRepository.GetLoggedInClient().IsAdmin;
        }

        public int GetLoggedInClientId()
        {
            return _clientRepository.GetLoggedInClientId();
        }

        public Client GetLoggedInClient()
        {
            return _clientRepository.GetLoggedInClient();
        }

        public bool IsLoggedInClientAdmin()
        {
            return _clientRepository.IsLoggedInClientAdmin();
        }

        public List<Client> GetAllClientsAndSortByAdminAndBanned()
        {
            return _clientRepository.GetAllClientsAndSortByAdminAndBanned();
        }

        public bool ExistsClientByEmail(string email)
        {
            return _clientRepository.ExistsByEmail(email);
        }

        public bool ExistsClientByLogin(string login)
        {
            return _clientRepository.GetClientByLogin(login) != null;
        }
    }
}
