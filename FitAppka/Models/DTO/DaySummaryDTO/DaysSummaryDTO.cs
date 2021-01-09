using System;
using System.Collections.Generic;

namespace FitnessApp.Models.DTO.DaySummaryDTO
{
    public class DaysSummaryDTO
    {
        public List<DayDTO> Days { get; set; }
        public double SumOfKcalConsumed { get; set; }
        public double SumOfProteinsConsumed { get; set; }
        public double SumOfFatsConsumed { get; set; }
        public double SumOfCarbsConsumed { get; set; }
        public int SumOfWaterDrunk { get; set; }
        public double SumOfKcalGoals { get; set; }
        public double SumOfProteinsGoals { get; set; }
        public double SumOfFatsGoals { get; set; }
        public double SumOfCarbsGoals { get; set; }
    }
}
