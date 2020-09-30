using System;
using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<CardioTraining> CardioTraining { get; set; }
        public virtual DbSet<CardioTrainingType> CardioTrainingType { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Day> Day { get; set; }
        public virtual DbSet<Diet> Diet { get; set; }
        public virtual DbSet<DietProduct> DietProduct { get; set; }
        public virtual DbSet<FatMeasurement> FatMeasurement { get; set; }
        public virtual DbSet<Goals> Goals { get; set; }
        public virtual DbSet<Meal> Meal { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<StrengthTraining> StrengthTraining { get; set; }
        public virtual DbSet<StrengthTrainingType> StrengthTrainingType { get; set; }
        public virtual DbSet<WeightMeasurement> WeightMeasurement { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-P22JPL7\\SQLEXPRESS;Initial Catalog=FitAppDiet;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardioTraining>(entity =>
            {
                entity.HasIndex(e => e.CardioTrainingTypeId)
                    .HasName("r10_FK");

                entity.HasIndex(e => e.DayId)
                    .HasName("r9_FK");

                entity.HasOne(d => d.CardioTrainingType)
                    .WithMany(p => p.CardioTraining)
                    .HasForeignKey(d => d.CardioTrainingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CARDIOTR_R10_CARDIOTR");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.CardioTraining)
                    .HasForeignKey(d => d.DayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CARDIOTR_R9_DAY");
            });

            modelBuilder.Entity<CardioTrainingType>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("Cardio_author_FK");

                entity.Property(e => e.TrainingName).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.CardioTrainingType)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_CARDIOTR_CARDIO_AU_CLIENT");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.GoalsId)
                    .HasName("r13_FK");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.SecondName).IsUnicode(false);

                entity.HasOne(d => d.Goals)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.GoalsId)
                    .HasConstraintName("FK_CLIENT_R13_GOALS");
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("r1_FK");

                entity.HasIndex(e => e.GoalsId)
                    .HasName("r15_FK");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Day)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAY_R1_CLIENT");

                entity.HasOne(d => d.Goals)
                    .WithMany(p => p.Day)
                    .HasForeignKey(d => d.GoalsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DAY_R15_GOALS");
            });

            modelBuilder.Entity<Diet>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("Relationship_17_FK");

                entity.Property(e => e.DietName).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Diet)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DIET_RELATIONS_CLIENT");
            });

            modelBuilder.Entity<DietProduct>(entity =>
            {
                entity.HasIndex(e => e.DietId)
                    .HasName("Relationship_15_FK");

                entity.HasIndex(e => e.ProductId)
                    .HasName("Relationship_16_FK");

                entity.HasOne(d => d.Diet)
                    .WithMany(p => p.DietProduct)
                    .HasForeignKey(d => d.DietId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DIETPROD_RELATIONS_DIET");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DietProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DIETPROD_RELATIONS_PRODUCT");
            });

            modelBuilder.Entity<FatMeasurement>(entity =>
            {
                entity.HasIndex(e => e.WeightMeasurementId)
                    .HasName("r6_FK");

                entity.HasOne(d => d.WeightMeasurement)
                    .WithMany(p => p.FatMeasurement)
                    .HasForeignKey(d => d.WeightMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FAT_MEAS_R6_WEIGHT_M");
            });

            modelBuilder.Entity<Goals>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("r14_FK");

                entity.HasIndex(e => e.DayId)
                    .HasName("r16_FK");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.GoalsNavigation)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_GOALS_R14_CLIENT");

                entity.HasOne(d => d.DayNavigation)
                    .WithMany(p => p.GoalsNavigation)
                    .HasForeignKey(d => d.DayId)
                    .HasConstraintName("FK_GOALS_R16_DAY");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasIndex(e => e.DayId)
                    .HasName("r2_FK");

                entity.HasIndex(e => e.ProductId)
                    .HasName("r8_FK");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.Meal)
                    .HasForeignKey(d => d.DayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MEAL_R2_DAY");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Meal)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MEAL_R8_PRODUCT");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("Product_author_FK");

                entity.Property(e => e.PhotoPath).IsUnicode(false);

                entity.Property(e => e.ProductName).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_PRODUCT_PRODUCT_A_CLIENT");
            });

            modelBuilder.Entity<StrengthTraining>(entity =>
            {
                entity.HasIndex(e => e.DayId)
                    .HasName("r12_FK");

                entity.HasIndex(e => e.StrengthTrainingTypeId)
                    .HasName("Relationship_11_FK");

                entity.HasOne(d => d.Day)
                    .WithMany(p => p.StrengthTraining)
                    .HasForeignKey(d => d.DayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STRENGTH_R12_DAY");

                entity.HasOne(d => d.StrengthTrainingType)
                    .WithMany(p => p.StrengthTraining)
                    .HasForeignKey(d => d.StrengthTrainingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STRENGTH_RELATIONS_STRENGTH");
            });

            modelBuilder.Entity<StrengthTrainingType>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("Strength_training_author_FK");

                entity.Property(e => e.TrainingName).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.StrengthTrainingType)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_STRENGTH_STRENGTH__CLIENT");
            });

            modelBuilder.Entity<WeightMeasurement>(entity =>
            {
                entity.HasIndex(e => e.ClientId)
                    .HasName("r3_FK");

                entity.HasIndex(e => e.FatMeasurementId)
                    .HasName("r5_FK");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.WeightMeasurement)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WEIGHT_M_R3_CLIENT");

                entity.HasOne(d => d.FatMeasurementNavigation)
                    .WithMany(p => p.WeightMeasurementNavigation)
                    .HasForeignKey(d => d.FatMeasurementId)
                    .HasConstraintName("FK_WEIGHT_M_R5_FAT_MEAS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
