using FitnessApp.Models.DTO.DietCreatorDTO;
using System.Collections.Generic;

namespace FitnessApp.Models.DTO.EditDietDTO
{
    public class EditDietDTO
    {
        public DietDTO EditedDiet { get; set; }
        public List<SearchProductDTO> SearchProducts { get; set; }
        public List<DietProductDTO> AddedProducts { get; set; }
        public string RootPath { get; set; }
        public bool WasSearched { get; set; }
    }
}
