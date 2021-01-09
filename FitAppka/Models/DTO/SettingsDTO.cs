using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class SettingsDTO
    {
        [Required]
        [Display(Name = "Płeć")]
        public bool? Sex { get; set; }

        [Display(Name = "Data urodzenia")]
        [Required(ErrorMessage = "Proszę podać datę urodzenia")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth{ get; set; }

        [Display(Name = "Masa ciała")]
        [DataType(DataType.Currency, ErrorMessage = "Waga została podana nieprawidłowo")]
        [Required(ErrorMessage = "Prosze podać wagę (w kg)")]
        public double? Weight { get; set; }

        [Display(Name = "Wzrost")]
        [Required(ErrorMessage = "Proszę podać wzrost [w cm]")]
        public int? Growth { get; set; }

        [Display(Name = "Obecny cel zmian wagi")]
        [Required]
        public short WeightChangeGoal { get; set; }

        [Display(Name = "Tempo zmian wagi na tydzień")]
        public string PaceOfChanges { get; set; }

        [Display(Name = "Wybierz swój poziom aktywności")]
        public short? ActivityLevel { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? Dessert { get; set; }
        public bool? Snack { get; set; }
        public bool? Supper { get; set; }
    }
}
