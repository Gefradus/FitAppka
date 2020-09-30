using FitAppka.Models;
using System.Collections.Generic;
namespace FitAppka.Repository.RepoInterface
{
    public interface IDietProductRepository
    {
        DietProduct GetDietProduct(int id);
        IEnumerable<DietProduct> GetAllDietProducts();
        DietProduct Add(DietProduct dietProduct);
        DietProduct Update(DietProduct dietProduct);
        DietProduct Delete(int id);
    }
}
