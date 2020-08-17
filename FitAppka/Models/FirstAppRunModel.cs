using System;
using System.ComponentModel.DataAnnotations;

namespace FitAppka.Models
{
    public class FirstAppRunModel
    {
        [Required]
        public bool? Plec { get; set; }

        [Display(Name = "Data urodzenia")]
        [Required(ErrorMessage = "Proszę podać datę urodzenia")]
        [DataType(DataType.Date)]
        public DateTime? DataUrodzenia { get; set; }

        [Display(Name = "Masa ciała [kg]")]
        [DataType(DataType.Currency, ErrorMessage = "Waga została podana nieprawidłowo")]
        [Required(ErrorMessage = "Prosze podać wagę (w kg)")]
        public double? Waga { get; set; }

        [Display(Name = "Wzrost [cm]")]
        [Required(ErrorMessage = "Proszę podać wzrost [w cm]")]
        public int? Wzrost { get; set; }

        [Display(Name = "Obecny cel")]
        [Required]
        public short Cel { get; set; }

        [Display(Name = "Tempo zmian wagi na tydzień")]
        public string TempoZmian { get; set; }

        [Display(Name = "Wybierz swój poziom aktywności")]
        public short? Aktywnosc { get; set; }

        public bool? Sniadanie { get; set; }
        public bool? Iisniadanie { get; set; }
        public bool? Obiad { get; set; }
        public bool? Deser { get; set; }
        public bool? Przekaska { get; set; }
        public bool? Kolacja { get; set; }
    }
}
