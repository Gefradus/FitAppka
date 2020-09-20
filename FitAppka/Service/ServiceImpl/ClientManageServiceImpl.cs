using FitAppka.Models;
using FitAppka.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Service.ServiceImpl
{
    public class ClientManageServiceImpl : IClientManageService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWeightMeasurementRepository _weightMeasurementRepository;

        public ClientManageServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository,
            IProductRepository productRepository, IWeightMeasurementRepository weightMeasurementRepository)
        {
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
                DateOfJoining = DateTime.Now
            });
        }

        private Client GetClientFromModel(Client model)
        {
            string loginOrEmail = model.Login.ToLower();
            return _clientRepository.GetAllClients().FirstOrDefault(c => c.Login.ToLower().Equals(loginOrEmail) || c.Email.ToLower().Equals(loginOrEmail));
        }

        public string GetClientLoginFromModel(Client model)
        {
            return GetClientFromModel(model).Login;
        }

        public bool CheckIfPassCorrect(Client model)
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
    }
}
