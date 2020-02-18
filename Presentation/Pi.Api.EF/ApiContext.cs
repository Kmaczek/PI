using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pi.Api.EF.Models.Auth;

namespace Pi.Api.EF
{
    public partial class ApiContext : DbContext
    {
        private readonly IConfigurationRoot configuration;

        public ApiContext(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

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
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

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
