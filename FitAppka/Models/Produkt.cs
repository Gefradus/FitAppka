using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Produkt
    {
        public Produkt()
        {
            Posilek = new HashSet<Posilek>();
        }

        [Key]
        [Column("Produkt_ID")]
        public int ProduktId { get; set; }
        [Column("Nazwa_produktu")]
        [StringLength(50)]
        public string NazwaProduktu { get; set; }
        [Column("Zdjecie_sciezka")]
        [StringLength(512)]
        public string ZdjecieSciezka { get; set; }
        public double? Kalorie { get; set; }
        public double? Bialko { get; set; }
        public double? Tluszcze { get; set; }
        public double? Weglowodany { get; set; }
        [Column("Witamina_A")]
        public double? WitaminaA { get; set; }
        [Column("Witamina_C")]
        public double? WitaminaC { get; set; }
        [Column("Witamina_D")]
        public double? WitaminaD { get; set; }
        [Column("Witamina_K")]
        public double? WitaminaK { get; set; }
        [Column("Witamina_E")]
        public double? WitaminaE { get; set; }
        [Column("Witamina_B1")]
        public double? WitaminaB1 { get; set; }
        [Column("Witamina_B2")]
        public double? WitaminaB2 { get; set; }
        [Column("Witamina_B5")]
        public double? WitaminaB5 { get; set; }
        [Column("Witamina_B6")]
        public double? WitaminaB6 { get; set; }
        public double? Biotyna { get; set; }
        [Column("Kwas_foliowy")]
        public double? KwasFoliowy { get; set; }
        [Column("Witamina_B12")]
        public double? WitaminaB12 { get; set; }
        [Column("Witamina_PP")]
        public double? WitaminaPp { get; set; }
        public double? Cynk { get; set; }
        public double? Fosfor { get; set; }
        public double? Jod { get; set; }
        public double? Magnez { get; set; }
        public double? Miedz { get; set; }
        public double? Potas { get; set; }
        public double? Selen { get; set; }
        public double? Sod { get; set; }
        public double? Wapn { get; set; }
        public double? Zelazo { get; set; }

        [InverseProperty("Produkt")]
        public virtual ICollection<Posilek> Posilek { get; set; }
    }
}
