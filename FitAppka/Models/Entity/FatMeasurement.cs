using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    [Table("Fat_measurement")]
    public partial class FatMeasurement
    {
        public FatMeasurement()
        {
            WeightMeasurementNavigation = new HashSet<WeightMeasurement>();
        }

        [Key]
        [Column("Fat_measurement_ID")]
        public int FatMeasurementId { get; set; }
        [Column("Weight_measurement_ID")]
        public int WeightMeasurementId { get; set; }
        [Column("Waist_circumference")]
        public double? WaistCircumference { get; set; }
        [Column("Body_fat_level")]
        public double? BodyFatLevel { get; set; }

        [ForeignKey(nameof(WeightMeasurementId))]
        [InverseProperty("FatMeasurement")]
        public virtual WeightMeasurement WeightMeasurement { get; set; }
        [InverseProperty("FatMeasurementNavigation")]
        public virtual ICollection<WeightMeasurement> WeightMeasurementNavigation { get; set; }
    }
}
