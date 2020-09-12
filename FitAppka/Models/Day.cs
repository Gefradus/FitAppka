using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Day
    {
        public Day()
        {
            CardioTraining = new HashSet<CardioTraining>();
            Meal = new HashSet<Meal>();
            StrengthTraining = new HashSet<StrengthTraining>();
        }

        [Key]
        [Column("Day_ID")]
        public int DayId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("Calorie_goal")]
        public int? CalorieGoal { get; set; }
        [Column("Water_drunk")]
        public int? WaterDrunk { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? Dessert { get; set; }
        public bool? Snack { get; set; }
        public bool? Supper { get; set; }
        [Column("Protein_target")]
        public int? ProteinTarget { get; set; }
        [Column("Fat_target")]
        public int? FatTarget { get; set; }
        [Column("Carbs_target")]
        public int? CarbsTarget { get; set; }
        [Column("Kcal_burned_goal")]
        public int? KcalBurnedGoal { get; set; }
        [Column("Training_time_goal")]
        public int? TrainingTimeGoal { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("Day")]
        public virtual Client Client { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<CardioTraining> CardioTraining { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<Meal> Meal { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<StrengthTraining> StrengthTraining { get; set; }
    }
}
