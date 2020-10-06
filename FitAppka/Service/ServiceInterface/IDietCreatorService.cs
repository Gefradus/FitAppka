using FitAppka.Models;
using FitAppka.Models.DTO.DietCreatorDTO;
using System.Collections.Generic;

namespace FitAppka.Service.ServiceInterface
{
    public interface IDietCreatorService
    {
        public ActiveDietDTO GetActiveDiet(int dayOfWeek);
        public CreateDietDTO SearchProducts(List<DietProductDTO> addedProducts, string search);
        public CreateDietDTO AddProduct(List<DietProductDTO> addedProducts, int productId, int grammage);
    }
}
