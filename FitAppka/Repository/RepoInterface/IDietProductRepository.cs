using FitnessApp.Models;
using System.Collections.Generic;
namespace FitnessApp.Repository.RepoInterface
{
    public interface IDietProductRepository
    {
        DietProduct GetDietProduct(int id);
        List<DietProduct> GetAllDietProducts();
        List<DietProduct> GetDietProducts(int dietId);
        DietProduct Add(DietProduct dietProduct);
        DietProduct Update(DietProduct dietProduct);
        DietProduct Delete(int id);
    }
}
