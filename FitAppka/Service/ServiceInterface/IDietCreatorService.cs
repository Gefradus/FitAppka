using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using FitAppka.Models.DTO.EditDietDTO;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceInterface
{
    public interface IDietCreatorService
    {
        public ActiveDietDTO GetActiveDiet(int dayOfWeek);
        public CreateDietDTO SearchProducts(string search, bool wasSearched);
        public CreateDietDTO AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage, bool wasSearched);
        public CreateDietDTO DeleteProduct(List<DietProductDTO> addedProducts, int tempId);
        public bool DeleteDiet(int id);
        public EditDietDTO EditDietSearchProduct(int id, string search, bool wasSearched);
        public List<ActiveDietDTO> GetActiveDiets();
        public bool CreateDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding);
        public bool EditDiet(List<DietProductDTO> products, DietDTO dietDTO, bool overriding);
    }
}
