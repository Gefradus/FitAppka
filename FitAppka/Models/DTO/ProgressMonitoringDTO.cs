using FitAppka.Models.DTO;
using System.Collections.Generic;

namespace FitAppka.Models
{
    public class ProgressMonitoringDTO
    {
        public List<WeightMeasurementDTO> WeightMeasurements { get; set; }
        public List<CaloriesInDayDTO> CaloriesInDays { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public char ChartType { get; set; }

    }
}
