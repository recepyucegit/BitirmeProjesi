using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Table
        builder.ToTable("Customers");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.IdentityNumber)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.PostalCode)
            .HasMaxLength(10);

        builder.Property(c => c.CustomerType)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Individual");

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(c => c.IdentityNumber)
            .IsUnique();

        builder.HasIndex(c => c.Email);
        builder.HasIndex(c => c.Phone);
        builder.HasIndex(c => c.City);
        builder.HasIndex(c => c.CustomerType);
        builder.HasIndex(c => c.IsActive);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
