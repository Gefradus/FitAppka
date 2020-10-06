using System.Collections.Generic;

namespace FitAppka.Models
{
    public class ActiveDietDTO
    {
        public DietDTO Diet { get; set; }
        public List<DietProductDTO> Products { get; set; }
    }
}
