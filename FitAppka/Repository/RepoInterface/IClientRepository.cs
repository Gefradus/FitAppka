using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IClientRepository
    {
        Client GetClientById(int id);
        Client GetClientByEmail(string email);
        Client GetClientByLogin(string login);
        IEnumerable<Client> GetAllClients();
        Task<List<Client>> GetAllClientsAsync();
        Client Add(Client client);
        Task<Client> AddAsync(Client client);
        Client Update(Client client);
        Client Ban(int id);
        Client GetLoggedInClient();
        Client GetLoggedInClientAsNoTracking();
        int GetLoggedInClientId();
        public void BanClient(int id);
        public void UnbanClient(int id);
        bool IsLoggedInClientAdmin();
        public bool IsClientBanned(int clientId);
    }
}
