using System.Collections.Generic;

namespace FitAppka.Models
{
    public class DaysAndProductsDTO
    {
        public List<FindDayProductDTO> Products { get; set; }
        public List<DayDTO> Days { get; set; }
    }
}
