using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Repository
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        Product GetProductAsNoTracking(int id);
        IEnumerable<Product> GetAllProducts();
        Product Add(Product produkt);
        Product Update(Product produkt);
        Product Delete(int id);
    }
}
