using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class CardioTraining
    {
        [Key]
        [Column("CardioTraining_ID")]
        public int CardioTrainingId { get; set; }
        [Column("CardioTrainingType_ID")]
        public int CardioTrainingTypeId { get; set; }
        [Column("Day_ID")]
        public int DayId { get; set; }
        [Column("Time_in_minutes")]
        public int? TimeInMinutes { get; set; }
        [Column("Calories_burned")]
        public int? CaloriesBurned { get; set; }

        [ForeignKey(nameof(CardioTrainingTypeId))]
        [InverseProperty("CardioTraining")]
        public virtual CardioTrainingType CardioTrainingType { get; set; }
        [ForeignKey(nameof(DayId))]
        [InverseProperty("CardioTraining")]
        public virtual Day Day { get; set; }
    }
}
