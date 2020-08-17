using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitAppka.Models
{
    public partial class Klient
    {
        public Klient()
        {
            Dzien = new HashSet<Dzien>();
            PomiarTluszczu = new HashSet<PomiarTluszczu>();
            PomiarWagi = new HashSet<PomiarWagi>();
        }

        [Key]
        [Column("Klient_ID")]
        public int KlientId { get; set; }
        
        [StringLength(50)]
        public string Email { get; set; }
        
        [StringLength(50)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Należy podać hasło")]
        [StringLength(50)]
        public string Haslo { get; set; }
        [StringLength(50)]
        public string Imie { get; set; }
        [StringLength(50)]
        public string Nazwisko { get; set; }
        public bool CzyAdministrator { get; set; }
        public bool? Plec { get; set; }
        [Column("Data_urodzenia", TypeName = "datetime")]
        public DateTime? DataUrodzenia { get; set; }
        [Column("Data_dolaczenia", TypeName = "datetime")]
        public DateTime? DataDolaczenia { get; set; }
        public int? Wzrost { get; set; }
        [Column("Zapotrzebowanie_kcal")]
        public int? ZapotrzebowanieKcal { get; set; }
        [Column("Cel_zmian_wagi")]
        public short? CelZmianWagi { get; set; }
        [Column("Tempo_zmian")]
        public double? TempoZmian { get; set; }
        [Column("Poziom_aktywnosci")]
        public short? PoziomAktywnosci { get; set; }
        [Column("Cel_kalorii")]
        public int? CelKalorii { get; set; }
        [Column("Cel_bialko")]
        public int? CelBialko { get; set; }
        [Column("Cel_tluszcze")]
        public int? CelTluszcze { get; set; }
        [Column("Cel_wegl")]
        public int? CelWegl { get; set; }
        public bool? Sniadanie { get; set; }
        [Column("IISniadanie")]
        public bool? Iisniadanie { get; set; }
        public bool? Obiad { get; set; }
        public bool? Deser { get; set; }
        public bool? Przekaska { get; set; }
        public bool? Kolacja { get; set; }
        [Column("Cel_spalonych_kcal")]
        public int? CelSpalonychKcal { get; set; }
        [Column("Cel_min_treningow")]
        public int? CelMinTreningow { get; set; }

        [InverseProperty("Klient")]
        public virtual ICollection<Dzien> Dzien { get; set; }
        [InverseProperty("Klient")]
        public virtual ICollection<PomiarTluszczu> PomiarTluszczu { get; set; }
        [InverseProperty("Klient")]
        public virtual ICollection<PomiarWagi> PomiarWagi { get; set; }
    }
}
