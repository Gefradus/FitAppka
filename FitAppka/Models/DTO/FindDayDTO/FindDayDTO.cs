namespace FitAppka.Models
{
    public class FindDayDTO
    {
        public int FindBy { get; set; }
        public int ProductId { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
