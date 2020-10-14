using FitAppka.Models.DTO.DietCreatorDTO;
using System.Collections.Generic;

namespace FitAppka.Models.DTO.EditDietDTO
{
    public class EditDietDTO
    {
        public int Id { get; set; }
        public List<SearchProductDTO> SearchProducts { get; set; }
        public List<DietProductDTO> AddedProducts { get; set; }
        public string RootPath { get; set; }
        public bool WasSearched { get; set; }
    }
}
