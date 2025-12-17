using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
{
    public void Configure(EntityTypeBuilder<SaleDetail> builder)
    {
        // Table
        builder.ToTable("SaleDetails");

        // Primary Key
        builder.HasKey(sd => sd.Id);

        // Properties
        builder.Property(sd => sd.Quantity)
            .IsRequired();

        builder.Property(sd => sd.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(sd => sd.DiscountRate)
            .HasColumnType("decimal(5,2)")
            .HasDefaultValue(0);

        builder.Property(sd => sd.DiscountAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(sd => sd.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(sd => sd.NetPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(sd => sd.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(sd => sd.Sale)
            .WithMany(s => s.SaleDetails)
            .HasForeignKey(sd => sd.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sd => sd.Product)
            .WithMany()
            .HasForeignKey(sd => sd.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(sd => sd.SaleId);
        builder.HasIndex(sd => sd.ProductId);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(sd => !sd.IsDeleted);
    }
}
