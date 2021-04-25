using X.PagedList;

namespace FitnessApp.Models.DTO
{
    public class AdminProductDTO
    {
        public IPagedList<ProductDTO> Products { get; set; }
        public string? Search { get; set; }
    }
}
