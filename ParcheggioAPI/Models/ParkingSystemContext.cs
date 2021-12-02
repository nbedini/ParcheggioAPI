using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ParcheggioAPI.Models
{
    public partial class ParkingSystemContext : DbContext
    {
        public ParkingSystemContext()
        {
        }

        public ParkingSystemContext(DbContextOptions<ParkingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Parking> Parkings { get; set; }
        public virtual DbSet<ParkingAmount> ParkingAmounts { get; set; }
        public virtual DbSet<ParkingCost> ParkingCosts { get; set; }
        public virtual DbSet<ParkingHistory> ParkingHistorys { get; set; }
        public virtual DbSet<ParkingStatuss> ParkingStatusses { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=79.56.131.249\\SQLEXPRESS,50200;Database=ParkingSystem;User Id=sa;Password=NikoBDIta2002;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Parking>(entity =>
            {
                entity.HasKey(e => e.NomeParcheggio);

                entity.Property(e => e.NomeParcheggio)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Colonne)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Righe)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParkingAmount>(entity =>
            {
                entity.HasKey(e => e.Giorno);

                entity.Property(e => e.Giorno).HasColumnType("datetime");

                entity.Property(e => e.IncassoTotale)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomeParcheggio)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.NomeParcheggioNavigation)
                    .WithMany(p => p.ParkingAmounts)
                    .HasForeignKey(d => d.NomeParcheggio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingAmounts_Parkings");
            });

            modelBuilder.Entity<ParkingCost>(entity =>
            {
                entity.HasKey(e => e.TipoVeicolo);

                entity.Property(e => e.TipoVeicolo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParkingHistory>(entity =>
            {
                entity.HasKey(e => e.Idstorico);

                entity.Property(e => e.Idstorico).HasColumnName("IDStorico");

                entity.Property(e => e.Colonna)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DataOrarioEntrata).HasColumnType("datetime");

                entity.Property(e => e.DataOrarioUscita).HasColumnType("datetime");

                entity.Property(e => e.NomeParcheggio)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Propietario)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Riga)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Targa)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.TipoVeicolo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.PropietarioNavigation)
                    .WithMany(p => p.ParkingHistories)
                    .HasForeignKey(d => d.Propietario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingHistorys_Persons");

                entity.HasOne(d => d.TipoVeicoloNavigation)
                    .WithMany(p => p.ParkingHistories)
                    .HasForeignKey(d => d.TipoVeicolo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingHistorys_ParkingCosts");
            });

            modelBuilder.Entity<ParkingStatuss>(entity =>
            {
                entity.HasKey(e => e.Targa);

                entity.ToTable("ParkingStatuss");

                entity.Property(e => e.Targa)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Colonna)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DataOrarioEntrata).HasColumnType("datetime");

                entity.Property(e => e.NomeParcheggio)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Riga)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TipoVeicolo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.NomeParcheggioNavigation)
                    .WithMany(p => p.ParkingStatusses)
                    .HasForeignKey(d => d.NomeParcheggio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingStatuss_Parkings");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.CodiceFiscale);

                entity.Property(e => e.CodiceFiscale)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Cognome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataNascita).HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Targa)
                    .HasName("PK__Vehicles__6C5E0D6A06C413BB");

                entity.Property(e => e.Targa)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Modello)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Propietario)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TipoVeicolo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.PropietarioNavigation)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.Propietario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicles_Persons");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
