using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepoImpl
{
    public class SQLDietRepository : IDietRepository

    {
        private readonly IClientRepository _clientRepository;
        private readonly FitAppContext _context;
        public SQLDietRepository(FitAppContext context, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _context = context;
        }

        public Diet Add(Diet diet)
        {
            _context.Add(diet);
            _context.SaveChanges();
            return diet;
        }

        public Diet Delete(int id)
        {
            Diet diet = GetDiet(id);

            if (diet != null)
            {
                _context.Diet.Remove(diet);
                _context.SaveChanges();
            }

            return diet;
        }

        public IEnumerable<Diet> GetAllDiets()
        {
            return _context.Diet.ToList();
        }

        public Diet GetDiet(int id)
        {
            return _context.Diet.Find(id);
        }

        public List<Diet> GetLoggedInClientDiets()
        {
            return _context.Diet.Where(d => d.ClientId == _clientRepository.GetLoggedInClientId()).ToList();
        }

        public Diet Update(Diet diet)
        {
            _context.Update(diet);
            _context.SaveChanges();
            return diet;
        }
    }
}
