using FitAppka.Models;
using FitAppka.Repository.RepoInterface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Repository.RepIfaceImpl
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly FitAppContext _context;
        private readonly IClientRepository _clientRepository;

        public SQLProductRepository(FitAppContext context, IClientRepository clientRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
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

            if (product != null) {
                product.IsDeleted = true;
                _context.SaveChanges();
            }

            return product;
        }


        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Product.ToList();
        }

        public List<Product> SearchProducts(string search)
        {
            return _context.Product.Where(p => (p.ProductName.Contains(search) || string.IsNullOrEmpty(search)) 
                && (p.ClientId == _clientRepository.GetLoggedInClientId() || _clientRepository.IsLoggedInClientAdmin()
                || p.VisibleToAll) && p.IsDeleted == false).ToList();
        }

        public List<Product> GetAccessedToLoggedInClientProducts()
        {
            return _context.Product.Where(p => (p.ClientId == _clientRepository.GetLoggedInClientId() || 
                _clientRepository.IsLoggedInClientAdmin() || p.VisibleToAll) && p.IsDeleted == false).ToList();
        }

        public List<Product> GetLoggedInClientProducts()
        {
            return _context.Product.Where(p => p.ClientId == _clientRepository.GetLoggedInClientId() && p.IsDeleted == false).ToList();
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

        public Product GetProductAsNoTracking(int id)
        {
            return _context.Product.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
        }

        
    }
}
