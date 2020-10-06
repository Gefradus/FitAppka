namespace FitAppka.Models.DTO.DietCreatorDTO
{
    public class SearchProductDTO
    {
        public int ProductId { get; set; }
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
    }
}
