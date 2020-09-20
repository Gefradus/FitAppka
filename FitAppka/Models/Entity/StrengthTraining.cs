using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class StrengthTraining
    {
        [Key]
        [Column("StrengthTraining_ID")]
        public int StrengthTrainingId { get; set; }
        [Column("StrengthTrainingType_ID")]
        public int StrengthTrainingTypeId { get; set; }
        [Column("Day_ID")]
        public int DayId { get; set; }
        public short Repetitions { get; set; }
        public short Sets { get; set; }
        public short? Weight { get; set; }

        [ForeignKey(nameof(DayId))]
        [InverseProperty("StrengthTraining")]
        public virtual Day Day { get; set; }
        [ForeignKey(nameof(StrengthTrainingTypeId))]
        [InverseProperty("StrengthTraining")]
        public virtual StrengthTrainingType StrengthTrainingType { get; set; }
    }
}
