using FitnessApp.Models;
using FitnessApp.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Repository.RepoImpl
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
                diet.IsDeleted = true;
                _context.SaveChanges();
            }

            return diet;
        }

        public Task<List<Diet>> GetActiveDiets()
        {
            return _context.Diet.ToListAsync();
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
            return _context.Diet.Where(d => d.ClientId == _clientRepository.GetLoggedInClientId() && d.IsDeleted == false).ToList();
        }


        public Diet Update(Diet diet)
        {
            _context.Update(diet);
            _context.SaveChanges();
            return diet;
        }
    }
}
