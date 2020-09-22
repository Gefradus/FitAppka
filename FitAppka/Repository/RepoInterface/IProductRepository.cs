using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        Product GetProductAsNoTracking(int id);
        IEnumerable<Product> GetAllProducts();
        Task<List<Product>> SearchProducts(string search);
        Product Add(Product produkt);
        Product Update(Product produkt);
        Product Delete(int id);
    }
}
