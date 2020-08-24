using FitAppka.Models;
using System;
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

        public Client Delete(int id)
        {
            Client client = GetClient(id);

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

        public Client GetClient(int id)
        {
            return _context.Client.Find(id);
        }

        public Client Update(Client client)
        {
            _context.Update(client);
            _context.SaveChanges();
            return client;
        }
    }
}
