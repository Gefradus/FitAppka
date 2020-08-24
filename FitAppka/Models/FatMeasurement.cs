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
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column("Waist_circumference")]
        public double? WaistCircumference { get; set; }
        [Column("Body_fat_level")]
        public double? BodyFatLevel { get; set; }
        [Column("Date_of_measurement", TypeName = "datetime")]
        public DateTime? DateOfMeasurement { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("FatMeasurement")]
        public virtual Client Client { get; set; }
        [ForeignKey(nameof(WeightMeasurementId))]
        [InverseProperty("FatMeasurement")]
        public virtual WeightMeasurement WeightMeasurement { get; set; }
        [InverseProperty("FatMeasurementNavigation")]
        public virtual ICollection<WeightMeasurement> WeightMeasurementNavigation { get; set; }
    }
}
