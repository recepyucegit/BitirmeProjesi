using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SupplierTransactionDetailConfiguration : IEntityTypeConfiguration<SupplierTransactionDetail>
{
    public void Configure(EntityTypeBuilder<SupplierTransactionDetail> builder)
    {
        // Table
        builder.ToTable("SupplierTransactionDetails");

        // Primary Key
        builder.HasKey(std => std.Id);

        // Properties
        builder.Property(std => std.Quantity)
            .IsRequired();

        builder.Property(std => std.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(std => std.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(std => std.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(std => std.SupplierTransaction)
            .WithMany(st => st.Details)
            .HasForeignKey(std => std.SupplierTransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(std => std.Product)
            .WithMany()
            .HasForeignKey(std => std.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(std => std.SupplierTransactionId);
        builder.HasIndex(std => std.ProductId);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(std => !std.IsDeleted);
    }
}
