using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    [Table("Pomiar_wagi")]
    public partial class PomiarWagi
    {
        public PomiarWagi()
        {
            PomiarTluszczu = new HashSet<PomiarTluszczu>();
        }

        [Key]
        [Column("Pomiar_wagi_ID")]
        public int PomiarWagiId { get; set; }
        [Column("Pomiar_tluszczu_ID")]
        public int? PomiarTluszczuId { get; set; }
        [Column("Klient_ID")]
        public int KlientId { get; set; }
        public double Waga { get; set; }
        [Column("Data_pomiaru", TypeName = "datetime")]
        public DateTime? DataPomiaru { get; set; }

        [ForeignKey(nameof(KlientId))]
        [InverseProperty("PomiarWagi")]
        public virtual Klient Klient { get; set; }
        [ForeignKey(nameof(PomiarTluszczuId))]
        [InverseProperty("PomiarWagiNavigation")]
        public virtual PomiarTluszczu PomiarTluszczuNavigation { get; set; }
        [InverseProperty("PomiarWagi")]
        public virtual ICollection<PomiarTluszczu> PomiarTluszczu { get; set; }
    }
}
