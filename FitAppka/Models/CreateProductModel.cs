using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FitAppka.Models
{
    public class CreateProductModel
    {
        [Required(ErrorMessage = "Podaj nazwê produktu")]
        [MaxLength(25, ErrorMessage = "Maksymalna d³ugoœæ nazwy to 30 znaków")]
        public string NazwaProduktu { get; set; }
        public IFormFile Zdjecie { get; set; }
        [Required(ErrorMessage = "Podaj kalorycznoœæ w 100g")]
        [Range(0, 10000, ErrorMessage = "Iloœæ kcal musi wynosiæ max. 10000")]
        public double? Kalorie { get; set; }
        [Required(ErrorMessage = "Podaj bia³ko w 100g")]
        [Range(0, 100, ErrorMessage = "Iloœæ bia³ka musi wynosiæ max. 100g")]
        public double? Bialko { get; set; }
        [Required(ErrorMessage = "Podaj t³uszcze w 100g")]
        [Range(0, 100, ErrorMessage = "Iloœæ t³uszczy musi wynosiæ max. 100g")]
        public double? Tluszcze { get; set; }
        [Required(ErrorMessage = "Podaj wêglowodany w 100g")]
        [Range(0, 100, ErrorMessage = "Iloœæ wêgl. musi wynosiæ max. 100g")]
        public double? Weglowodany { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. A mo¿e wynosiæ max. 9999")]
        public double? WitaminaA { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. C mo¿e wynosiæ max. 9999")]
        public double? WitaminaC { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. D mo¿e wynosiæ max. 9999")]
        public double? WitaminaD { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. K mo¿e wynosiæ max. 9999")]
        public double? WitaminaK { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. E mo¿e wynosiæ max. 9999")]
        public double? WitaminaE { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. B1 mo¿e wynosiæ max. 9999")]
        public double? WitaminaB1 { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. B2 mo¿e wynosiæ max. 9999")]
        public double? WitaminaB2 { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. B5 mo¿e wynosiæ max. 9999")]
        public double? WitaminaB5 { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. B6 mo¿e wynosiæ max. 9999")]
        public double? WitaminaB6 { get; set; }
        [Range(0, 9999, ErrorMessage = "Biotyna mo¿e wynosiæ max. 9999")]
        public double? Biotyna { get; set; }
        [Range(0, 9999, ErrorMessage = "Kwas foliowy mo¿e wynosiæ max. 9999")]
        public double? KwasFoliowy { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. B12 mo¿e wynosiæ max. 9999")]
        public double? WitaminaB12 { get; set; }
        [Range(0, 9999, ErrorMessage = "Wit. PP mo¿e wynosiæ max. 9999")]
        public double? WitaminaPp { get; set; }
        [Range(0, 9999, ErrorMessage = "Cynk mo¿e wynosiæ max. 9999")]
        public double? Cynk { get; set; }
        [Range(0, 9999, ErrorMessage = "Fosfor mo¿e wynosiæ max. 9999")]
        public double? Fosfor { get; set; }
        [Range(0, 9999, ErrorMessage = "Jod mo¿e wynosiæ max. 9999")]
        public double? Jod { get; set; }
        [Range(0, 9999, ErrorMessage = "Magnez mo¿e wynosiæ max. 9999")]
        public double? Magnez { get; set; }
        [Range(0, 9999, ErrorMessage = "MiedŸ mo¿e wynosiæ max. 9999")]
        public double? Miedz { get; set; }
        [Range(0, 9999, ErrorMessage = "Potas mo¿e wynosiæ max. 9999")]
        public double? Potas { get; set; }
        [Range(0, 9999, ErrorMessage = "Selen mo¿e wynosiæ max. 9999")]
        public double? Selen { get; set; }
        [Range(0, 9999, ErrorMessage = "Sód mo¿e wynosiæ max. 9999")]
        public double? Sod { get; set; }
        [Range(0, 9999, ErrorMessage = "Wapñ mo¿e wynosiæ max. 9999")]
        public double? Wapn { get; set; }
        [Range(0, 9999, ErrorMessage = "¯elazo mo¿e wynosiæ max. 9999")]
        public double? Zelazo { get; set; }
    }
}