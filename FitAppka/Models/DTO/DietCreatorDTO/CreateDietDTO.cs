using System.Collections.Generic;

namespace FitnessApp.Models.DTO.DietCreatorDTO
{
    public class CreateDietDTO
    {
        public List<SearchProductDTO> SearchProducts { get; set; }
        public List<DietProductDTO> AddedProducts { get; set; }
        public string RootPath { get; set; }
        public bool WasSearched { get; set; }
    }
}
