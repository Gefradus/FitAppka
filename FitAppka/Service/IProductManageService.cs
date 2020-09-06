using FitAppka.Models;

namespace FitAppka.Service
{
    public interface IProductManageService
    {
        public string MealName(int inWhichMeal);
        public string DayPattern(int dayID);
        public void CreateProductFromModel(CreateProductModel model);
        public void UpdateProduct(CreateProductModel model, int id, int addOrEditPhoto);
    }
}
