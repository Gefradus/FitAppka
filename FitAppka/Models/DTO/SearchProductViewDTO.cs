using System.Collections.Generic;

namespace FitnessApp.Models.DTO
{
    public class SearchProductViewDTO
    {
        public SearchProductDTO SearchDTO { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
