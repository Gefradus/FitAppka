using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace FitAppka.Models
{
    public class CreateProductModel
    {
        [Required(ErrorMessage = "Podaj nazwę produktu")]
        [MaxLength(25, ErrorMessage = "Maksymalna długość nazwy to 25 znaków")]
        public string NazwaProduktu { get; set; }
        public IFormFile Zdjecie { get; set; }
        [Required(ErrorMessage = "Podaj kaloryczność w 100g")]
        [Range(0, 10000, ErrorMessage = "Ilość kcal musi wynosić max. 10000")]
        public double? Kalorie { get; set; }
        [Required(ErrorMessage = "Podaj białko w 100g")]
        [Range(0, 100, ErrorMessage = "Ilość białka musi wynosić max. 100g")]
        public double? Bialko { get; set; }
        [Required(ErrorMessage = "Podaj tłuszcze w 100g")]
        [Range(0, 100, ErrorMessage = "Ilość tłuszczy musi wynosić max. 100g")]
        public double? Tluszcze { get; set; }
        [Required(ErrorMessage = "Podaj węglowodany w 100g")]
        [Range(0, 100, ErrorMessage = "Ilośćść węgl. musi wynosić max. 100g")]
        public double? Weglowodany { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. A może wynosić max. 10000")]
        public double? WitaminaA { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. C może wynosić max. 10000")]
        public double? WitaminaC { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. D może wynosić max. 10000")]
        public double? WitaminaD { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. K może wynosić max. 10000")]
        public double? WitaminaK { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. E może wynosić max. 10000")]
        public double? WitaminaE { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B1 może wynosić max. 10000")]
        public double? WitaminaB1 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B2 może wynosić max. 10000")]
        public double? WitaminaB2 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B5 może wynosić max. 10000")]
        public double? WitaminaB5 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B6 może wynosić max. 10000")]
        public double? WitaminaB6 { get; set; }
        [Range(0, 10000, ErrorMessage = "Biotyna może wynosić max. 10000")]
        public double? Biotyna { get; set; }
        [Range(0, 10000, ErrorMessage = "Kwas foliowy może wynosić max. 10000")]
        public double? KwasFoliowy { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B12 może wynosić max. 10000")]
        public double? WitaminaB12 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. PP może wynosić max. 10000")]
        public double? WitaminaPp { get; set; }
        [Range(0, 10000, ErrorMessage = "Cynk może wynosić max. 10000")]
        public double? Cynk { get; set; }
        [Range(0, 10000, ErrorMessage = "Fosfor może wynosić max. 10000")]
        public double? Fosfor { get; set; }
        [Range(0, 10000, ErrorMessage = "Jod może wynosić max. 10000")]
        public double? Jod { get; set; }
        [Range(0, 10000, ErrorMessage = "Magnez może wynosić max. 10000")]
        public double? Magnez { get; set; }
        [Range(0, 10000, ErrorMessage = "Miedź może wynosić max. 10000")]
        public double? Miedz { get; set; }
        [Range(0, 10000, ErrorMessage = "Potas może wynosić max. 10000")]
        public double? Potas { get; set; }
        [Range(0, 10000, ErrorMessage = "Selen może wynosić max. 10000")]
        public double? Selen { get; set; }
        [Range(0, 10000, ErrorMessage = "Sód może wynosić max. 10000")]
        public double? Sod { get; set; }
        [Range(0, 10000, ErrorMessage = "Wapń może wynosić max. 10000")]
        public double? Wapn { get; set; }
        [Range(0, 10000, ErrorMessage = "Żelazo może wynosić max. 10000")]
        public double? Zelazo { get; set; }
    }
}