using FitAppka.Models;
using System.Collections.Generic;

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
        List<ProductDTO> SearchProduct(string search, bool onlyUserItem, int dayId, bool onlyFromDiet);
    }
}
