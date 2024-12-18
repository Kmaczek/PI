using Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pi.Api.EF.Configurations.Price
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductDm>
    {
        public void Configure(EntityTypeBuilder<ProductDm> builder)
        {
            builder.ToTable("Product", "price");

            builder.Property(e => e.ActiveFrom).HasColumnType("datetime");
            builder.Property(e => e.ActiveTo).HasColumnType("datetime");
            builder.Property(e => e.CreatedDate).HasColumnType("datetime");
            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            builder.Property(e => e.Uri)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.HasOne(d => d.LatestPriceDetail)
                .WithMany()
                .HasForeignKey(d => d.LatestPriceDetailId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_PriceDetail_Product");
        }
    }
}
