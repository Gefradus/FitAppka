using System;
using System.ComponentModel.DataAnnotations;

namespace FitAppka.Models
{
    public class SettingsModel
    {
        [Required]
        public bool? Sex { get; set; }

        [Display(Name = "Data urodzenia")]
        [Required(ErrorMessage = "Proszę podać datę urodzenia")]
        [DataType(DataType.Date)]
        public DateTime? Date_of_birth { get; set; }

        [Display(Name = "Masa ciała [kg]")]
        [DataType(DataType.Currency, ErrorMessage = "Waga została podana nieprawidłowo")]
        [Required(ErrorMessage = "Prosze podać wagę (w kg)")]
        public double? Weight { get; set; }

        [Display(Name = "Wzrost [cm]")]
        [Required(ErrorMessage = "Proszę podać wzrost [w cm]")]
        public int? Growth { get; set; }

        [Display(Name = "Obecny cel")]
        [Required]
        public short WeightChange_Goal { get; set; }

        [Display(Name = "Tempo zmian wagi na tydzień")]
        public string PaceOfChange { get; set; }

        [Display(Name = "Wybierz swój poziom aktywności")]
        public short? LevelOfActivity { get; set; }
        public bool? Breakfast { get; set; }
        public bool? Lunch { get; set; }
        public bool? Dinner { get; set; }
        public bool? Dessert { get; set; }
        public bool? Snack { get; set; }
        public bool? Supper { get; set; }
    }
}
