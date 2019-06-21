using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Data.EF.Models
{
    public partial class PiContext : DbContext
    {
        private readonly IConfigurationRoot configuration;

        public PiContext(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public PiContext(DbContextOptions<PiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BinanceSeries> BinanceSeries { get; set; }
        public virtual DbSet<SeriesParent> SeriesParent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetSection("PI_DbConnectionString").Value);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<BinanceSeries>(entity =>
            {
                entity.ToTable("Series", "binance");

                entity.Property(e => e.Ammount).HasColumnType("money");

                entity.Property(e => e.AvgPrice).HasColumnType("money");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.SeriesParent)
                    .WithMany(p => p.Series)
                    .HasForeignKey(d => d.SeriesParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeriesParent_Series");
            });

            modelBuilder.Entity<SeriesParent>(entity =>
            {
                entity.ToTable("SeriesParent", "binance");

                entity.Property(e => e.FetchedDate).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("money");
            });
        }
    }
}
