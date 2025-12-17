using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SupplierTransactionConfiguration : IEntityTypeConfiguration<SupplierTransaction>
{
    public void Configure(EntityTypeBuilder<SupplierTransaction> builder)
    {
        // Table
        builder.ToTable("SupplierTransactions");

        // Primary Key
        builder.HasKey(st => st.Id);

        // Properties
        builder.Property(st => st.OrderDate)
            .IsRequired();

        builder.Property(st => st.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(st => st.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Ordered");

        builder.Property(st => st.Notes)
            .HasMaxLength(1000);

        builder.Property(st => st.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(st => st.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(st => st.Supplier)
            .WithMany()
            .HasForeignKey(st => st.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(st => st.Employee)
            .WithMany()
            .HasForeignKey(st => st.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(st => st.Details)
            .WithOne(d => d.SupplierTransaction)
            .HasForeignKey(d => d.SupplierTransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(st => st.OrderDate);
        builder.HasIndex(st => st.SupplierId);
        builder.HasIndex(st => st.EmployeeId);
        builder.HasIndex(st => st.Status);
        builder.HasIndex(st => st.OrderNumber)
            .IsUnique();

        // Query Filter - Soft Delete
        builder.HasQueryFilter(st => !st.IsDeleted);
    }
}
