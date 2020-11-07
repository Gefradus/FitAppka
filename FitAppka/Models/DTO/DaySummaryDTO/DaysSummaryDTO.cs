using System;
using System.Collections.Generic;

namespace FitAppka.Models.DTO.DaySummaryDTO
{
    public class DaysSummaryDTO
    {
        public List<DayDTO> Days { get; set; }
        public double SumOfKcalConsumed { get; set; }
        public double SumOfProteinsConsumed { get; set; }
        public double SumOfFatsConsumed { get; set; }
        public double SumOfCarbsConsumed { get; set; }
        public double SumOfKcalGoals { get; set; }
        public double SumOfProteinsGoals { get; set; }
        public double SumOfFatsGoals { get; set; }
        public double SumOfCarbsGoals { get; set; }
    }
}
