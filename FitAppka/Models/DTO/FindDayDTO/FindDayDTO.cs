using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FitAppka.Models
{
    public class FindDayDTO
    {
        public List<SelectListItem> Products { get; set; }
        public List<DayDTO> Days { get; set; }
        public bool WasSearchedFor { get; set; }
        public int FindBy { get; set; }
        public int ProductId { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
