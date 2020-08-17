using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Trening
    {
        [Key]
        [Column("Trening_ID")]
        public int TreningId { get; set; }
        [Column("Dzien_ID")]
        public int DzienId { get; set; }
        [Column("TreningRodzaj_ID")]
        public int TreningRodzajId { get; set; }
        [Column("Czas_w_minutach")]
        public int? CzasWMinutach { get; set; }
        [Column("Spalone_kalorie")]
        public int? SpaloneKalorie { get; set; }

        [ForeignKey(nameof(DzienId))]
        [InverseProperty("Trening")]
        public virtual Dzien Dzien { get; set; }
        [ForeignKey(nameof(TreningRodzajId))]
        [InverseProperty("Trening")]
        public virtual TreningRodzaj TreningRodzaj { get; set; }
    }
}
