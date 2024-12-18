using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pi.Api.EF.Models.Price;

namespace Pi.Api.EF.Configurations.Price
{
    public class PriceDetailsDmConfiguration : IEntityTypeConfiguration<PriceDetailsDm>
    {
        public void Configure(EntityTypeBuilder<PriceDetailsDm> builder)
        {
            builder.ToTable("PriceDetails", "price");

            builder.Property(e => e.CreatedDate).HasColumnType("datetime");

            builder.Property(e => e.RetailerNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

            builder.Property(e => e.Title)
                .HasMaxLength(500)
                .IsUnicode(false);
        }
    }
}
