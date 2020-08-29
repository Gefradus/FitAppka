using FitAppka.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public class SQLClientRepository : IClientRepository
    {
        private readonly FitAppContext _context;

        public SQLClientRepository(FitAppContext context)
        {
            _context = context;
        }

        public Client Add(Client client)
        {
            _context.Add(client);
            _context.SaveChanges();
            return client;
        }

        public async Task<Client> AddAsync(Client client)
        {
            _context.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public Client Delete(int id)
        {
            Client client = GetClientById(id);

            if (client != null)
            {
                _context.Client.Remove(client);
                _context.SaveChanges();
            }

            return client;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _context.Client.ToList();
        }

        public Task<List<Client>> GetAllClientsAsync()
        {
            return _context.Client.ToListAsync();
        }

        public Client GetClientByEmail(string email)
        {
            return _context.Client.Where(k => k.Email.ToLower().Equals(email.ToLower())).FirstOrDefault();
        }

        public Client GetClientById(int id)
        {
            return _context.Client.Find(id);
        }

        public Client GetClientByLogin(string login)
        {
            return _context.Client.Where(k => k.Login.ToLower().Equals(login.ToLower())).FirstOrDefault();
        }

        public Client Update(Client client)
        {
            _context.Update(client);
            _context.SaveChanges();
            return client;
        }
    }
}
