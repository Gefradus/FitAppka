
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
        Client Add(Client client);
        Task<Client> AddAsync(Client client);
        Client Update(Client client);
        Client Delete(int id);
    }
}
