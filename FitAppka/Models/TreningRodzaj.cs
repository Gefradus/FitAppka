using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class TreningRodzaj
    {
        public TreningRodzaj()
        {
            Trening = new HashSet<Trening>();
        }

        [Key]
        [Column("TreningRodzaj_ID")]
        public int TreningRodzajId { get; set; }
        [Column("Nazwa_treningu")]
        [StringLength(50)]
        public string NazwaTreningu { get; set; }
        [Column("Kcal_na_min")]
        public int? KcalNaMin { get; set; }

        [InverseProperty("TreningRodzaj")]
        public virtual ICollection<Trening> Trening { get; set; }
    }
}
