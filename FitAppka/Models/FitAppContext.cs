using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FitAppka.Models
{
    public partial class FitAppContext : DbContext
    {
        public FitAppContext()
        {
        }

        public FitAppContext(DbContextOptions<FitAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dzien> Dzien { get; set; }
        public virtual DbSet<Klient> Klient { get; set; }
        public virtual DbSet<PomiarTluszczu> PomiarTluszczu { get; set; }
        public virtual DbSet<PomiarWagi> PomiarWagi { get; set; }
        public virtual DbSet<Posilek> Posilek { get; set; }
        public virtual DbSet<Produkt> Produkt { get; set; }
        public virtual DbSet<Trening> Trening { get; set; }
        public virtual DbSet<TreningRodzaj> TreningRodzaj { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // Dyrektywa #warning
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-P22JPL7\\SQLEXPRESS;Initial Catalog=FitApp;Integrated Security=True");
#pragma warning restore CS1030 // Dyrektywa #warning
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dzien>(entity =>
            {
                entity.HasIndex(e => e.KlientId)
                    .HasName("r1_FK");

                entity.HasOne(d => d.Klient)
                    .WithMany(p => p.Dzien)
                    .HasForeignKey(d => d.KlientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DZIEN_R1_KLIENT");
            });

            modelBuilder.Entity<Klient>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Haslo).IsUnicode(false);

                entity.Property(e => e.Imie).IsUnicode(false);

                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Nazwisko).IsUnicode(false);
            });

            modelBuilder.Entity<PomiarTluszczu>(entity =>
            {
                entity.HasIndex(e => e.KlientId)
                    .HasName("r4_FK");

                entity.HasIndex(e => e.PomiarWagiId)
                    .HasName("r6_FK");

                entity.HasOne(d => d.Klient)
                    .WithMany(p => p.PomiarTluszczu)
                    .HasForeignKey(d => d.KlientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POMIAR_T_R4_KLIENT");

                entity.HasOne(d => d.PomiarWagi)
                    .WithMany(p => p.PomiarTluszczu)
                    .HasForeignKey(d => d.PomiarWagiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POMIAR_T_R6_POMIAR_W");
            });

            modelBuilder.Entity<PomiarWagi>(entity =>
            {
                entity.HasIndex(e => e.KlientId)
                    .HasName("r3_FK");

                entity.HasIndex(e => e.PomiarTluszczuId)
                    .HasName("r5_FK");

                entity.HasOne(d => d.Klient)
                    .WithMany(p => p.PomiarWagi)
                    .HasForeignKey(d => d.KlientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POMIAR_W_R3_KLIENT");

                entity.HasOne(d => d.PomiarTluszczuNavigation)
                    .WithMany(p => p.PomiarWagiNavigation)
                    .HasForeignKey(d => d.PomiarTluszczuId)
                    .HasConstraintName("FK_POMIAR_W_R5_POMIAR_T");
            });

            modelBuilder.Entity<Posilek>(entity =>
            {
                entity.HasIndex(e => e.DzienId)
                    .HasName("r2_FK");

                entity.HasIndex(e => e.ProduktId)
                    .HasName("r8_FK");

                entity.HasOne(d => d.Dzien)
                    .WithMany(p => p.Posilek)
                    .HasForeignKey(d => d.DzienId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSILEK_R2_DZIEN");

                entity.HasOne(d => d.Produkt)
                    .WithMany(p => p.Posilek)
                    .HasForeignKey(d => d.ProduktId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSILEK_R8_PRODUKT");
            });

            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.Property(e => e.NazwaProduktu).IsUnicode(false);

                entity.Property(e => e.ZdjecieSciezka).IsUnicode(false);
            });

            modelBuilder.Entity<Trening>(entity =>
            {
                entity.HasIndex(e => e.DzienId)
                    .HasName("r9_FK");

                entity.HasIndex(e => e.TreningRodzajId)
                    .HasName("r10_FK");

                entity.HasOne(d => d.Dzien)
                    .WithMany(p => p.Trening)
                    .HasForeignKey(d => d.DzienId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRENING_R9_DZIEN");

                entity.HasOne(d => d.TreningRodzaj)
                    .WithMany(p => p.Trening)
                    .HasForeignKey(d => d.TreningRodzajId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRENING_R10_TRENINGR");
            });

            modelBuilder.Entity<TreningRodzaj>(entity =>
            {
                entity.Property(e => e.NazwaTreningu).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
