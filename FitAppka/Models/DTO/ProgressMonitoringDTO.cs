using FitnessApp.Models.Enum;
using System.Collections.Generic;

namespace FitnessApp.Models
{
    public class ProgressMonitoringDTO
    {
        public List<MeasurementDTO> Measurements { get; set; }
        public List<ChartDataInDayDTO> ChartDataInDays { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public ChartTypeStrategyEnum ChartType { get; set; }

    }
}
