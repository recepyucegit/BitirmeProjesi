using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Barcode)
            .HasMaxLength(50);

        builder.Property(e => e.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.CostPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.StockQuantity)
            .HasDefaultValue(0);

        builder.Property(e => e.CriticalStockLevel)
            .HasDefaultValue(10);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(e => e.Barcode)
            .IsUnique()
            .HasFilter("[Barcode] IS NOT NULL");

        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.StockQuantity);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasOne(e => e.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
