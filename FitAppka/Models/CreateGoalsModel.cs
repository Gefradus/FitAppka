using System;
using System.ComponentModel.DataAnnotations;


namespace FitAppka.Models
{
    public class CreateGoalsModel
    {
        [Range(500, 20000, ErrorMessage = "Cel kalorii musi wynosić 500 - 20000")]
        public int? CalorieGoal { get; set; }

        [Range(0, 5000, ErrorMessage = "Cel białka może wynosić max. 5000")]
        public int? ProteinTarget { get; set; }

        [Range(0, 5000, ErrorMessage = "Cel węglowodanów może wynosić max. 5000")]
        public int? CarbsTarget { get; set; }

        [Range(0, 2222, ErrorMessage = "Cel tłuszczy może wynosić max. 2222")]
        public int? FatTarget { get; set; }

        [Range(0, 5000, ErrorMessage = "Cel spalonych kcal może wynosić max. 5000")]
        
        public int? KcalBurnedGoal { get; set; }

        [Range(0, 1440, ErrorMessage = "Cel minut treningów może wynosić max. 1440")]
        public int? TrainingTimeGoal { get; set; }

        [Required]
        public bool AutoDietaryGoals { get; set; }

        [Required]
        public bool IncludeCaloriesBurned { get; set; }

        [Range(0, 10000, ErrorMessage = "Cel wit. A może wynosić max. 1000000000000")]
        public double? VitaminA { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. C może wynosić max. 10000")]
        public double? VitaminC { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. D może wynosić max. 10000")]
        public double? VitaminD { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. K może wynosić max. 10000")]
        public double? VitaminK { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. E może wynosić max. 10000")]
        public double? VitaminE { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. B1 może wynosić max. 10000")]
        public double? VitaminB1 { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. B2 może wynosić max. 10000")]
        public double? VitaminB2 { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. B5 może wynosić max. 10000")]
        public double? VitaminB5 { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. B6 może wynosić max. 10000")]
        public double? VitaminB6 { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel biotyny może wynosić max. 10000")]
        public double? Biotin { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel kwasu foliowego może wynosić max. 10000")]
        public double? FolicAcid { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. B12 może wynosić max. 10000")]
        public double? VitaminB12 { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wit. PP może wynosić max. 10000")]
        public double? VitaminPp { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel cynku może wynosić max. 10000")]
        public double? Zinc { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel fosfor może wynosić max. 10000")]
        public double? Phosphorus { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel jod może wynosić max. 10000")]
        public double? Iodine { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel magnezu może wynosić max. 10000")]
        public double? Magnesium { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel miedzi może wynosić max. 10000")]
        public double? Copper { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel potasu może wynosić max. 10000")]
        public double? Potassium { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel selenu może wynosić max. 10000")]
        public double? Selenium { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel sodu może wynosić max. 10000")]
        public double? Sodium { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel wapnia może wynosić max. 10000")]
        public double? Calcium { get; set; }
        [Range(0, 10000, ErrorMessage = "Cel żelaza może wynosić max. 10000")]
        public double? Iron { get; set; }


    }
}
