namespace FitAppka.Models
{
    public class DietDTO
    {
        public int DietId { get; set; }
        public string DietName { get; set; }
        public bool Active { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}
