using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepoImpl
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

        public IEnumerable<DietProduct> GetAllDietProducts()
        {
            return _context.DietProduct.ToList();
        }

        public DietProduct GetDietProduct(int id)
        {
            return _context.DietProduct.Find(id);
        }

        public DietProduct Update(DietProduct dietProduct)
        {
            _context.Update(dietProduct);
            _context.SaveChanges();
            return dietProduct;
        }
    }
}
