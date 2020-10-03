namespace FitAppka.Models
{
    public class DietProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Grammage { get; set; }
        public string PhotoPath { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
    }

}
