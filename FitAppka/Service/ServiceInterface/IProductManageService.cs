using FitnessApp.Models;
using FitnessApp.Models.DTO;
using System.Collections.Generic;

namespace FitnessApp.Service
{
    public interface IProductManageService
    {
        string MealName(int inWhichMeal);
        string DayPattern(int dayID);
        void CreateProductFromModel(ProductDTO model);
        void UpdateProduct(ProductDTO model, int id, int addOrEditPhoto);
        Product GetProduct(int id);
        Product Delete(int id);
        SearchProductViewDTO Dto(SearchProductDTO dto);
        List<ProductDTO> SearchProduct(string search, bool onlyUserItem, int dayId, bool onlyFromDiet);
    }
}
