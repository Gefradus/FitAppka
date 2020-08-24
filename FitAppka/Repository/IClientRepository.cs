
using FitAppka.Models;
using System.Collections.Generic;


namespace FitAppka.Repository
{
    public interface IClientRepository
    {
        Client GetClient(int id);
        IEnumerable<Client> GetAllClients();
        Client Add(Client client);
        Client Update(Client client);
        Client Delete(int id);
    }
}
