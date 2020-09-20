using FitAppka.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Goals
    {
        public Goals()
        {
            Client = new HashSet<Client>();
            Day = new HashSet<Day>();
        }

        [Key]
        [Column("Goals_ID")]
        public int GoalsId { get; set; }
        [Column("Client_ID")]
        public int? ClientId { get; set; }
        [Column("Day_ID")]
        public int? DayId { get; set; }
        [Column("Kcal_burned")]
        public int? KcalBurned { get; set; }
        [Column("Training_time")]
        public int? TrainingTime { get; set; }
        public double Calories { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbohydrates { get; set; }
        [Column("Vitamin_A")]
        public double? VitaminA { get; set; }
        [Column("Vitamin_C")]
        public double? VitaminC { get; set; }
        [Column("Vitamin_D")]
        public double? VitaminD { get; set; }
        [Column("Vitamin_K")]
        public double? VitaminK { get; set; }
        [Column("Vitamin_E")]
        public double? VitaminE { get; set; }
        [Column("Vitamin_B1")]
        public double? VitaminB1 { get; set; }
        [Column("Vitamin_B2")]
        public double? VitaminB2 { get; set; }
        [Column("Vitamin_B5")]
        public double? VitaminB5 { get; set; }
        [Column("Vitamin_B6")]
        public double? VitaminB6 { get; set; }
        public double? Biotin { get; set; }
        [Column("Folic_acid")]
        public double? FolicAcid { get; set; }
        [Column("Vitamin_B12")]
        public double? VitaminB12 { get; set; }
        [Column("Vitamin_PP")]
        public double? VitaminPp { get; set; }
        public double? Zinc { get; set; }
        public double? Phosphorus { get; set; }
        public double? Iodine { get; set; }
        public double? Magnesium { get; set; }
        public double? Copper { get; set; }
        public double? Potassium { get; set; }
        public double? Selenium { get; set; }
        public double? Sodium { get; set; }
        public double? Calcium { get; set; }
        public double? Iron { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("GoalsNavigation")]
        public virtual Client ClientNavigation { get; set; }
        [ForeignKey(nameof(DayId))]
        [InverseProperty("GoalsNavigation")]
        public virtual Day DayNavigation { get; set; }
        [InverseProperty("Goals")]
        public virtual ICollection<Client> Client { get; set; }
        [InverseProperty("Goals")]
        public virtual ICollection<Day> Day { get; set; }
    }
}
