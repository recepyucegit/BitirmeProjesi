using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        // Table
        builder.ToTable("Sales");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.DiscountAmount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(s => s.NetAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.CommissionAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Cash");

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Completed");

        builder.Property(s => s.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Employee)
            .WithMany()
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.SaleDetails)
            .WithOne(sd => sd.Sale)
            .HasForeignKey(sd => sd.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.SaleDate);
        builder.HasIndex(s => s.CustomerId);
        builder.HasIndex(s => s.EmployeeId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.InvoiceNumber)
            .IsUnique();

        // Query Filter - Soft Delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
