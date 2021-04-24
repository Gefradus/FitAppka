namespace FitnessApp.Models.DTO
{
    public class SearchProductDTO
    {
        public string Day { get; set; }
        public string MealName { get; set; }
        public string ContentRootPath { get; set; }
        public int DayId { get; set; }
        public int AtWhich { get; set; }
        public string Search { get; set; }
        public bool WereSearched { get; set; }
        public bool OnlyUserItem { get; set; }
        public bool OnlyFromDiet { get; set; }
        public int? Page { get; set; }

    }
}
