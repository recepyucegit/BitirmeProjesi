using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        // Table
        builder.ToTable("Suppliers");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.ContactName)
            .HasMaxLength(100);

        builder.Property(e => e.ContactTitle)
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20);

        builder.Property(e => e.TaxNumber)
            .HasMaxLength(50);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(e => e.TaxNumber)
            .IsUnique()
            .HasFilter("[TaxNumber] IS NOT NULL");

        builder.HasIndex(e => e.CompanyName);
        builder.HasIndex(e => e.IsActive);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasMany(e => e.Products)
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
