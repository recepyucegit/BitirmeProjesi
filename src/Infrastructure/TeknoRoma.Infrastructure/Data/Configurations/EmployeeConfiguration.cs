using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // Table
        builder.ToTable("Employees");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Address)
            .HasMaxLength(500);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.IdentityNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.Property(e => e.Salary)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.SalesQuota)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(10000);

        builder.Property(e => e.CommissionRate)
            .HasColumnType("decimal(5,4)")
            .HasDefaultValue(0.10m);

        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.HasIndex(e => e.IdentityNumber)
            .IsUnique();

        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.Role);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.StoreId);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
