
using X.PagedList;

namespace FitnessApp.Models.DTO
{
    public class SearchProductViewDTO
    {
        public SearchProductDTO SearchDTO { get; set; }
        public IPagedList<ProductDTO> Products { get; set; }
    }
}
