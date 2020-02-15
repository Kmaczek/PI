using Data.EF.Models.Auth;
using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<AdditionalInfo> AdditionalInfo { get; set; }
        public virtual DbSet<Flat> Flat { get; set; }
        public virtual DbSet<FlatAdditionalInfo> FlatAdditionalInfo { get; set; }
        public virtual DbSet<FlatCategoty> FlatCategoty { get; set; }
        public virtual DbSet<FlatSeries> FlatSeries { get; set; }
        public virtual DbSet<FormOfProperty> FormOfProperty { get; set; }
        public virtual DbSet<Heating> Heating { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Market> Market { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<SeriesParent> SeriesParent { get; set; }
        public virtual DbSet<TypeOfBuilding> TypeOfBuilding { get; set; }

        public virtual DbSet<AppUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetSection("PI_DbConnectionString").Value);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<AdditionalInfo>(entity =>
            {
                entity.ToTable("AdditionalInfo", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Flat>(entity =>
            {
                entity.ToTable("Flat", "otodom");

                entity.HasIndex(e => e.OtoDomId)
                    .HasName("Flat_OtodomId_IDX");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.OtoDomId)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.Property(e => e.Rent).HasColumnType("smallmoney");

                entity.Property(e => e.Surface).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("money");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FormOfProperty)
                    .WithMany(p => p.Flat)
                    .HasForeignKey(d => d.FormOfPropertyId)
                    .HasConstraintName("FK_FormOfProperty_Flat");

                entity.HasOne(d => d.Heating)
                    .WithMany(p => p.Flat)
                    .HasForeignKey(d => d.HeatingId)
                    .HasConstraintName("FK_Heating_Flat");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Flat)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Location_Flat");

                entity.HasOne(d => d.Market)
                    .WithMany(p => p.Flat)
                    .HasForeignKey(d => d.MarketId)
                    .HasConstraintName("FK_Market_Flat");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Flat)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_TypeOfBuilding_Flat");
            });

            modelBuilder.Entity<FlatAdditionalInfo>(entity =>
            {
                entity.ToTable("FlatAdditionalInfo", "otodom");

                entity.HasOne(d => d.AdditionalInfo)
                    .WithMany(p => p.FlatAdditionalInfo)
                    .HasForeignKey(d => d.AdditionalInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionalInfo_FlatAdditionalInfo");

                entity.HasOne(d => d.Flat)
                    .WithMany(p => p.FlatAdditionalInfo)
                    .HasForeignKey(d => d.FlatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Flat_FlatAdditionalInfo");
            });

            modelBuilder.Entity<FlatCategoty>(entity =>
            {
                entity.ToTable("FlatCategoty", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FlatSeries>(entity =>
            {
                entity.ToTable("FlatSeries", "otodom");

                entity.Property(e => e.AvgPrice).HasColumnType("money");

                entity.Property(e => e.AvgPricePerMeter).HasColumnType("money");

                entity.Property(e => e.DateFetched).HasColumnType("datetime");

                entity.HasOne(d => d.BestValue)
                    .WithMany(p => p.FlatSeriesBestValue)
                    .HasForeignKey(d => d.BestValueId)
                    .HasConstraintName("FK_BestValue_FlatSeries");

                entity.HasOne(d => d.Biggest)
                    .WithMany(p => p.FlatSeriesBiggest)
                    .HasForeignKey(d => d.BiggestId)
                    .HasConstraintName("FK_Biggest_FlatSeries");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.FlatSeries)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Category_FlatSeries");

                entity.HasOne(d => d.Cheapest)
                    .WithMany(p => p.FlatSeriesCheapest)
                    .HasForeignKey(d => d.CheapestId)
                    .HasConstraintName("FK_Cheapest_FlatSeries");

                entity.HasOne(d => d.MostExpensive)
                    .WithMany(p => p.FlatSeriesMostExpensive)
                    .HasForeignKey(d => d.MostExpensiveId)
                    .HasConstraintName("FK_MostExpensive_FlatSeries");

                entity.HasOne(d => d.Smallest)
                    .WithMany(p => p.FlatSeriesSmallest)
                    .HasForeignKey(d => d.SmallestId)
                    .HasConstraintName("FK_Smallest_FlatSeries");
            });

            modelBuilder.Entity<FormOfProperty>(entity =>
            {
                entity.ToTable("FormOfProperty", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Heating>(entity =>
            {
                entity.ToTable("Heating", "otodom");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Market>(entity =>
            {
                entity.ToTable("Market", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Series>(entity =>
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

            modelBuilder.Entity<TypeOfBuilding>(entity =>
            {
                entity.ToTable("TypeOfBuilding", "otodom");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("User", "auth");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);
                    //.Metadata.IsIndex();

                entity.HasIndex(x => x.Username).IsUnique();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ActiveFrom)
                    .HasColumnType("datetime");

                entity.Property(e => e.ActiveTo)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime");
            });
        }
    }
}
