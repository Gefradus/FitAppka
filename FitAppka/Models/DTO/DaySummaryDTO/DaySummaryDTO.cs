using System;

namespace FitAppka.Models.DTO
{
    public class DaySummaryDTO
    {
        public DateTime Date { get; set; }
        
        public int KcalConsumed { get; set; }
        public double ProteinsConsumed { get; set; }
        public double FatsConsumed { get; set; }
        public double CarbohydratesConsumed { get; set; }
        public int WaterDrunk { get; set; }

        public int KcalGoal { get; set; }
        public double ProteinsGoal { get; set; }
        public double FatsGoal { get; set; }
        public double CarbohydratesGoal { get; set; }
    }
}
