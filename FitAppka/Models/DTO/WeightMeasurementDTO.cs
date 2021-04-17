using System;

namespace FitnessApp.Models
{
    public class WeightMeasurementDTO
    {
        public int WeightMeasurementId { get; set; }
        public double Measurement { get; set; }
        public DateTime DateOfMeasurement { get; set; }
    }
}
