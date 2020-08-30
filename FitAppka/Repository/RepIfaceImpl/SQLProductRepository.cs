using FitAppka.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly FitAppContext _context;

        public SQLProductRepository(FitAppContext context)
        {
            _context = context;
        }

        public Product Add(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Delete(int id)
        {
            Product product = GetProduct(id);
            DeleteRelatedMeals(id);

            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }

            return product;
        }


        private void DeleteRelatedMeals(int productID)
        {
            List<Meal> relatedMeals = _context.Meal.Where(p => p.ProductId == productID).ToList();

            foreach (var meal in relatedMeals)
            {
                _context.Meal.Remove(meal);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Product.ToList();
        }

        public Product GetProduct(int id)
        {
            return _context.Product.Find(id);
        }

        public Product Update(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}
