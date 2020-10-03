using System.Collections.Generic;

namespace FitAppka.Models
{
    public class DietCreatorDTO
    {
        public DietDTO Diet { get; set; }
        public List<DietProductDTO> Products { get; set; }
    }
}
