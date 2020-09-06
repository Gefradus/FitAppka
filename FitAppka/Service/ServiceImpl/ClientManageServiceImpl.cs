using FitAppka.Models;
using FitAppka.Repository;
using System.Linq;

namespace FitAppka.Service.ServiceImpl
{
    public class ClientManageServiceImpl : IClientManageService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDayRepository _dayRepository;

        public ClientManageServiceImpl(IClientRepository clientRepository, IDayRepository dayRepository)
        {
            _dayRepository = dayRepository;
            _clientRepository = clientRepository;
        }

        public async void AddNewClient(RegisterModel model)
        {
            await _clientRepository.AddAsync(new Client()
            {
                Login = model.Login,
                Email = model.Email,
                Password = model.Password,
                FirstName = model.FirstName,
                SecondName = model.SecondName
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

        public bool HasUserAccess(int dayID)
        {
            return _dayRepository.GetDay(dayID).ClientId == _clientRepository.GetLoggedInClientId() || _clientRepository.GetLoggedInClient().IsAdmin;
        }
        
    }
}
