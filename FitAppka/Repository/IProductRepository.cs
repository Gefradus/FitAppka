using FitAppka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitAppka.Repository
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product Add(Product produkt);
        Product Update(Product produkt);
        Product Delete(int id);
    }
}
