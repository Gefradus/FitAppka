using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Client
    {
        public Client()
        {
            CardioTrainingType = new HashSet<CardioTrainingType>();
            Day = new HashSet<Day>();
            Diet = new HashSet<Diet>();
            GoalsNavigation = new HashSet<Goals>();
            Product = new HashSet<Product>();
            StrengthTrainingType = new HashSet<StrengthTrainingType>();
            WeightMeasurement = new HashSet<WeightMeasurement>();
        }

        [Key]
        [Column("Client_ID")]
        public int ClientId { get; set; }
        [Column("Goals_ID")]
        public int? GoalsId { get; set; }
        
        [StringLength(50)]
        public string Email { get; set; }
        
        [StringLength(50)]
        public string Login { get; set; }
        
        [StringLength(50)]
        public string Password { get; set; }
        [Column("First_name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column("Second_name")]
        [StringLength(50)]
        public string SecondName { get; set; }
        public bool IsAdmin { get; set; }
        public bool? Sex { get; set; }
        [Column("Date_of_birth", TypeName = "datetime")]
        public DateTime? DateOfBirth { get; set; }
        [Column("Date_of_joining", TypeName = "datetime")]
        public DateTime? DateOfJoining { get; set; }
        public int? Growth { get; set; }
        [Column("Caloric_demand")]
        public int? CaloricDemand { get; set; }
        [Column("Weight_change_goal")]
        public short? WeightChangeGoal { get; set; }
        [Column("Pace_of_changes")]
        public double? PaceOfChanges { get; set; }
        [Column("Activity_level")]
        public short? ActivityLevel { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? Dessert { get; set; }
        public bool? Snack { get; set; }
        public bool? Supper { get; set; }
        [Column("Include_calories_burned")]
        public bool? IncludeCaloriesBurned { get; set; }
        [Column("Auto_dietary_goals")]
        public bool? AutoDietaryGoals { get; set; }
        public bool? IsBanned { get; set; }

        [ForeignKey(nameof(GoalsId))]
        [InverseProperty("Client")]
        public virtual Goals Goals { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<CardioTrainingType> CardioTrainingType { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<Day> Day { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<Diet> Diet { get; set; }
        [InverseProperty("ClientNavigation")]
        public virtual ICollection<Goals> GoalsNavigation { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<Product> Product { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<StrengthTrainingType> StrengthTrainingType { get; set; }
        [InverseProperty("Client")]
        public virtual ICollection<WeightMeasurement> WeightMeasurement { get; set; }
    }
}
