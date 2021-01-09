using System.Collections.Generic;

namespace FitnessApp.Models
{
    public class ActiveDietDTO
    {
        public DietDTO Diet { get; set; }
        public List<DietProductDTO> Products { get; set; }
        public int CaloriesSum { get; set; }
        public double ProteinsSum { get; set; }
        public double FatsSum { get; set; }
        public double CarbohydratesSum { get; set; }
    }
}
