namespace FitnessApp.Models.DTO
{
    public class SearchProductDTO
    {
        public int DayId { get; set; }
        public int AtWhich { get; set; }
        public string Search { get; set; }
        public bool WereSearched { get; set; }
        public string Day { get; set; }
        public string MealName { get; set; }
        public string ContentRootPath { get; set; }


    }
}
