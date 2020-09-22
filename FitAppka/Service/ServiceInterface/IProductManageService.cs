using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka.Service
{
    public interface IProductManageService
    {
        public string MealName(int inWhichMeal);
        public string DayPattern(int dayID);
        public void CreateProductFromModel(ProductDTO model);
        public void UpdateProduct(ProductDTO model, int id, int addOrEditPhoto);
        Product GetProduct(int id);
        Product Delete(int id);
        Task<List<ProductDTO>> SearchProduct(string search);
    }
}
