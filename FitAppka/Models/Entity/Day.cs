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
            GoalsNavigation = new HashSet<Goals>();
            Meal = new HashSet<Meal>();
            StrengthTraining = new HashSet<StrengthTraining>();
        }

        [Key]
        [Column("Day_ID")]
        public int DayId { get; set; }
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column("Goals_ID")]
        public int GoalsId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("Water_drunk")]
        public int? WaterDrunk { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? Dessert { get; set; }
        public bool? Snack { get; set; }
        public bool? Supper { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("Day")]
        public virtual Client Client { get; set; }
        [ForeignKey(nameof(GoalsId))]
        [InverseProperty("Day")]
        public virtual Goals Goals { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<CardioTraining> CardioTraining { get; set; }
        [InverseProperty("DayNavigation")]
        public virtual ICollection<Goals> GoalsNavigation { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<Meal> Meal { get; set; }
        [InverseProperty("Day")]
        public virtual ICollection<StrengthTraining> StrengthTraining { get; set; }
    }
}
