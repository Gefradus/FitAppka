using FitAppka.Model;
using System;
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
        Client Delete(int id);
        Client GetLoggedInClient();
        int GetLoggedInClientId();
        bool IsLoggedInClientAdmin();
    }
}
