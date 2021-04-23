using FitnessApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Repository.RepIfaceImpl
{
    public class SQLClientRepository : IClientRepository
    {
        private readonly FitAppContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SQLClientRepository(FitAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

        public Client Ban(int id)
        {
            Client client = GetClientById(id);

            if (client != null)
            {
                client.IsBanned = true;
                _context.SaveChanges();
            }

            return client;
        }

        public bool IsClientBanned(int clientId)
        {
            return GetClientById(clientId).IsBanned;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _context.Client.ToList();
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Client.FirstOrDefault(k => k.Email.ToLower().Equals(email.ToLower())) != null;
        }

        public Client GetClientById(int id)
        {
            return _context.Client.Find(id);
        }

        public Client GetClientByLogin(string login)
        {
            return _context.Client.FirstOrDefault(k => k.Login.ToLower().Equals(login.ToLower()));
        }

        public Client GetLoggedInClient()
        {
            return GetClientByLogin(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Client GetLoggedInClientAsNoTracking()
        {
            return _context.Client.AsNoTracking().FirstOrDefault(k => k.Login.ToLower().Equals(_httpContextAccessor.HttpContext.User.Identity.Name.ToLower()));
        }

        public int GetLoggedInClientId()
        {
            return GetClientByLogin(_httpContextAccessor.HttpContext.User.Identity.Name).ClientId;
        }

        public bool IsLoggedInClientAdmin()
        {
            return GetClientByLogin(_httpContextAccessor.HttpContext.User.Identity.Name).IsAdmin;
        }

        public Client Update(Client client)
        {
            _context.Update(client);
            _context.SaveChanges();
            return client;
        }

        public void BanClient(int id)
        {
            GetClientById(id).IsBanned = true;
            _context.SaveChanges();
        }

        public void UnbanClient(int id)
        {
            GetClientById(id).IsBanned = false;
            _context.SaveChanges();
        }

        public List<Client> GetAllClientsAndSortByAdminAndBanned()
        {
            return _context.Client.OrderBy(c => !c.IsAdmin).ThenBy(c => !c.IsBanned).ThenBy(c => c.DateOfJoining).ToList();
        }
    }
}
