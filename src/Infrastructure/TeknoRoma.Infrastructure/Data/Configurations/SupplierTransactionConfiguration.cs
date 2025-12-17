using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class SupplierTransactionConfiguration : IEntityTypeConfiguration<SupplierTransaction>
{
    public void Configure(EntityTypeBuilder<SupplierTransaction> builder)
    {
        builder.ToTable("SupplierTransactions");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.TransactionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(st => st.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(st => st.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(st => st.Description)
            .HasMaxLength(500);

        builder.Property(st => st.TransactionDate)
            .IsRequired();

        builder.Property(st => st.InvoiceNumber)
            .HasMaxLength(50);

        builder.Property(st => st.ReferenceNumber)
            .HasMaxLength(50);

        builder.Property(st => st.Status)
            .IsRequired()
            .HasMaxLength(50);

        // Relationships
        builder.HasOne(st => st.Supplier)
            .WithMany(s => s.SupplierTransactions)
            .HasForeignKey(st => st.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(st => st.Product)
            .WithMany(p => p.SupplierTransactions)
            .HasForeignKey(st => st.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(st => st.SupplierId);
        builder.HasIndex(st => st.ProductId);
        builder.HasIndex(st => st.TransactionDate);
        builder.HasIndex(st => st.InvoiceNumber);
    }
}
