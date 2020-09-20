using FitAppka.Models;
using System.Collections.Generic;

namespace FitAppka.Service
{
    public interface IProductManageService
    {
        public string MealName(int inWhichMeal);
        public string DayPattern(int dayID);
        public void CreateProductFromModel(CreateProductModel model);
        public void UpdateProduct(CreateProductModel model, int id, int addOrEditPhoto);
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product Add(Product product);
        Product Update(Product product);
        Product Delete(int id);
    }
}
