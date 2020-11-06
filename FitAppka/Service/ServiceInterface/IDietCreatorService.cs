using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using FitAppka.Models.DTO.EditDietDTO;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceInterface
{
    public interface IDietCreatorService
    {
        public ActiveDietDTO GetActiveDiet(int dayOfWeek, bool wasDayChanged);
        public CreateDietDTO SearchProducts(string search, bool wasSearched);
        public CreateDietDTO AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage, bool wasSearched);
        public CreateDietDTO DeleteProduct(List<DietProductDTO> addedProducts, int tempId);
        public bool DeleteDiet(int id);
        public EditDietDTO EditDietSearchProduct(int id, string search, bool wasSearched);
        public List<ActiveDietDTO> GetLoggedInClientActiveDiets();
        public bool CreateDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding);
        public bool EditDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding);
        public int CountCaloriesSum(List<DietProduct> list);
        public List<DietProductDTO> MapDietProductsToDTO(List<DietProduct> dietProducts);
        public List<DietProductDTO> MapProductsToDietProductsDTO(List<DietProductDTO> productsDTO);

    }
}
