using FitnessApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Repository
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        Product GetProductAsNoTracking(int id);
        IEnumerable<Product> GetAllProducts();
        List<Product> GetAccessedToLoggedInClientProducts();
        List<Product> SearchProducts(string search);
        List<Product> GetLoggedInClientProducts();
        Product Add(Product produkt);
        Product Update(Product produkt);
        Product Delete(int id);
    }
}
