using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Dzien
    {
        public Dzien()
        {
            Posilek = new HashSet<Posilek>();
            Trening = new HashSet<Trening>();
        }

        [Key]
        [Column("Dzien_ID")]
        public int DzienId { get; set; }
        [Column("Klient_ID")]
        public int KlientId { get; set; }
        [Column("Dzien", TypeName = "datetime")]
        public DateTime? Dzien1 { get; set; }
        [Column("Cel_kalorii")]
        public int? CelKalorii { get; set; }
        [Column("Wypita_woda")]
        public int? WypitaWoda { get; set; }
        public bool? Sniadanie { get; set; }
        [Column("IISniadanie")]
        public bool? Iisniadanie { get; set; }
        public bool? Obiad { get; set; }
        public bool? Deser { get; set; }
        public bool? Przekaska { get; set; }
        public bool? Kolacja { get; set; }
        [Column("Cel_bialko")]
        public int? CelBialko { get; set; }
        [Column("Cel_tluszcze")]
        public int? CelTluszcze { get; set; }
        [Column("Cel_wegl")]
        public int? CelWegl { get; set; }
        [Column("Cel_spalonych_kcal")]
        public int? CelSpalonychKcal { get; set; }
        [Column("Cel_min_treningow")]
        public int? CelMinTreningow { get; set; }

        [ForeignKey(nameof(KlientId))]
        [InverseProperty("Dzien")]
        public virtual Klient Klient { get; set; }
        [InverseProperty("Dzien")]
        public virtual ICollection<Posilek> Posilek { get; set; }
        [InverseProperty("Dzien")]
        public virtual ICollection<Trening> Trening { get; set; }
    }
}
