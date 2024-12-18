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

        public virtual DbSet<AppUserDm> ApplicationUsers { get; set; }

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

            modelBuilder.Entity<AppUserDm>(entity =>
            {
                entity.ToTable("User", "auth");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Id).IsUnique();

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

                entity.HasIndex(e => e.Email)
                    .HasDatabaseName("UQ__User__Email")
                    .IsUnique();

                entity.Property(e => e.ActiveFromUtc)
                    .HasColumnType("datetime");

                entity.Property(e => e.ActiveToUtc)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedDateUtc)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.LastLoginUtc)
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<RoleDm>(entity =>
            {
                entity.ToTable("Role", "auth");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserRoleDm>(entity =>
            {
                entity.ToTable("UserRoles", "auth");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_User");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoles_Role");
            });
        }
    }
}
