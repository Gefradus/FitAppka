using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    [Table("Pomiar_tluszczu")]
    public partial class PomiarTluszczu
    {
        public PomiarTluszczu()
        {
            PomiarWagiNavigation = new HashSet<PomiarWagi>();
        }

        [Key]
        [Column("Pomiar_tluszczu_ID")]
        public int PomiarTluszczuId { get; set; }
        [Column("Klient_ID")]
        public int KlientId { get; set; }
        [Column("Pomiar_wagi_ID")]
        public int PomiarWagiId { get; set; }
        [Column("Obwod_w_talii")]
        public double? ObwodWTalii { get; set; }
        [Column("Zawartosc_tluszczu")]
        public double? ZawartoscTluszczu { get; set; }
        [Column("Data_pomiaru", TypeName = "datetime")]
        public DateTime? DataPomiaru { get; set; }

        [ForeignKey(nameof(KlientId))]
        [InverseProperty("PomiarTluszczu")]
        public virtual Klient Klient { get; set; }
        [ForeignKey(nameof(PomiarWagiId))]
        [InverseProperty("PomiarTluszczu")]
        public virtual PomiarWagi PomiarWagi { get; set; }
        [InverseProperty("PomiarTluszczuNavigation")]
        public virtual ICollection<PomiarWagi> PomiarWagiNavigation { get; set; }
    }
}
