using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitAppka.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Podaj nazwę produktu")]
        [MaxLength(25, ErrorMessage = "Maksymalna długość nazwy to 25 znaków")]
        public string ProductName { get; set; }

        public IFormFile Photo { get; set; }

        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "Podaj kaloryczność w 100g")]
        [Range(0, 900, ErrorMessage = "Ilość kcal musi wynosić max. 900")]
        public double? Calories { get; set; }

        [Required(ErrorMessage = "Podaj białko w 100g")]
        [Range(0, 100, ErrorMessage = "Ilość białka musi wynosić max. 100g")]
        public double? Proteins { get; set; }

        [Required(ErrorMessage = "Podaj tłuszcze w 100g")]
        [Range(0, 100, ErrorMessage = "Ilość tłuszczy musi wynosić max. 100g")]
        public double? Fats { get; set; }

        [Required(ErrorMessage = "Podaj węglowodany w 100g")]
        [Range(0, 100, ErrorMessage = "Ilość węgl. musi wynosić max. 100g")]
        public double? Carbohydrates { get; set; }

        [Range(0, 10000, ErrorMessage = "Wit. A może wynosić max. 10000")]
        public double? VitaminA { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. C może wynosić max. 10000")]
        public double? VitaminC { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. D może wynosić max. 10000")]
        public double? VitaminD { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. K może wynosić max. 10000")]
        public double? VitaminK { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. E może wynosić max. 10000")]
        public double? VitaminE { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B1 może wynosić max. 10000")]
        public double? VitaminB1 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B2 może wynosić max. 10000")]
        public double? VitaminB2 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B5 może wynosić max. 10000")]
        public double? VitaminB5 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B6 może wynosić max. 10000")]
        public double? VitaminB6 { get; set; }
        [Range(0, 10000, ErrorMessage = "Biotyna może wynosić max. 10000")]
        public double? Biotin { get; set; }
        [Range(0, 10000, ErrorMessage = "Kwas foliowy może wynosić max. 10000")]
        public double? FolicAcid { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. B12 może wynosić max. 10000")]
        public double? VitaminB12 { get; set; }
        [Range(0, 10000, ErrorMessage = "Wit. PP może wynosić max. 10000")]
        public double? VitaminPp { get; set; }
        [Range(0, 10000, ErrorMessage = "Cynk może wynosić max. 10000")]
        public double? Zinc { get; set; }
        [Range(0, 10000, ErrorMessage = "Fosfor może wynosić max. 10000")]
        public double? Phosphorus { get; set; }
        [Range(0, 10000, ErrorMessage = "Jod może wynosić max. 10000")]
        public double? Iodine { get; set; }
        [Range(0, 10000, ErrorMessage = "Magnez może wynosić max. 10000")]
        public double? Magnesium { get; set; }
        [Range(0, 10000, ErrorMessage = "Miedź może wynosić max. 10000")]
        public double? Copper { get; set; }
        [Range(0, 10000, ErrorMessage = "Potas może wynosić max. 10000")]
        public double? Potassium { get; set; }
        [Range(0, 10000, ErrorMessage = "Selen może wynosić max. 10000")]
        public double? Selenium { get; set; }
        [Range(0, 10000, ErrorMessage = "Sód może wynosić max. 10000")]
        public double? Sodium { get; set; }
        [Range(0, 10000, ErrorMessage = "Wapń może wynosić max. 10000")]
        public double? Calcium { get; set; }
        [Range(0, 10000, ErrorMessage = "Żelazo może wynosić max. 10000")]
        public double? Iron { get; set; }
        public bool Eaten { get; set; }
    }
}