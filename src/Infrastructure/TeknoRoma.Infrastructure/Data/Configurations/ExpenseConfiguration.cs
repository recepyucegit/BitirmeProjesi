using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        // Table
        builder.ToTable("Expenses");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.ExpenseType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("TL");

        builder.Property(e => e.ExchangeRate)
            .HasColumnType("decimal(18,4)")
            .HasDefaultValue(1);

        builder.Property(e => e.AmountInTL)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.ExpenseDate)
            .IsRequired();

        builder.Property(e => e.InvoiceNumber)
            .HasMaxLength(50);

        builder.Property(e => e.Vendor)
            .HasMaxLength(200);

        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("BankTransfer");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Pending");

        builder.Property(e => e.Notes)
            .HasMaxLength(2000);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Store)
            .WithMany()
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Approver)
            .WithMany()
            .HasForeignKey(e => e.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(e => e.ExpenseType);
        builder.HasIndex(e => e.Category);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.ExpenseDate);
        builder.HasIndex(e => e.EmployeeId);
        builder.HasIndex(e => e.StoreId);
        builder.HasIndex(e => e.InvoiceNumber);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
