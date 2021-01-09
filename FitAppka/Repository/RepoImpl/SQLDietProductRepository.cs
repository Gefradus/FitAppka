using FitnessApp.Models;
using FitnessApp.Repository.RepoInterface;
using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Repository.RepoImpl
{
    public class SQLDietProductRepository : IDietProductRepository
    {
        private readonly FitAppContext _context;

        public SQLDietProductRepository(FitAppContext context)
        {
            _context = context;
        }

        public DietProduct Add(DietProduct dietProduct)
        {
            _context.Add(dietProduct);
            _context.SaveChanges();
            return dietProduct;
        }

        public DietProduct Delete(int id)
        {
            DietProduct dietProduct = GetDietProduct(id);

            if (dietProduct != null)
            {
                _context.DietProduct.Remove(dietProduct);
                _context.SaveChanges();
            }

            return dietProduct;
        }

        public List<DietProduct> GetAllDietProducts()
        {
            return _context.DietProduct.ToList();
        }

        public DietProduct GetDietProduct(int id)
        {
            return _context.DietProduct.Find(id);
        }

        public List<DietProduct> GetDietProducts(int dietId)
        {
            return _context.DietProduct.Where(p => p.DietId == dietId).ToList();
        }

        public DietProduct Update(DietProduct dietProduct)
        {
            _context.Update(dietProduct);
            _context.SaveChanges();
            return dietProduct;
        }
    }
}
