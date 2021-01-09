using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    [Table("Weight_measurement")]
    public partial class WeightMeasurement
    {
        public WeightMeasurement()
        {
            FatMeasurement = new HashSet<FatMeasurement>();
        }

        [Key]
        [Column("Weight_measurement_ID")]
        public int WeightMeasurementId { get; set; }
        [Column("Fat_measurement_ID")]
        public int? FatMeasurementId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        public double Weight { get; set; }
        [Column("Date_of_measurement", TypeName = "datetime")]
        public DateTime? DateOfMeasurement { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("WeightMeasurement")]
        public virtual Client Client { get; set; }
        [ForeignKey(nameof(FatMeasurementId))]
        [InverseProperty("WeightMeasurementNavigation")]
        public virtual FatMeasurement FatMeasurementNavigation { get; set; }
        [InverseProperty("WeightMeasurement")]
        public virtual ICollection<FatMeasurement> FatMeasurement { get; set; }
    }
}
