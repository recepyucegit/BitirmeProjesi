using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        // Table
        builder.ToTable("Stores");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.StoreName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.StoreCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.City)
            .HasMaxLength(50);

        builder.Property(s => s.District)
            .HasMaxLength(50);

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.Property(s => s.Email)
            .HasMaxLength(100);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);

        builder.Property(s => s.OpeningDate)
            .IsRequired();

        builder.Property(s => s.MonthlyTarget)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(s => s.StoreCode)
            .IsUnique();

        builder.HasIndex(s => s.City);
        builder.HasIndex(s => s.IsActive);
        builder.HasIndex(s => s.ManagerId);

        // Relationships
        builder.HasOne(s => s.Manager)
            .WithMany()
            .HasForeignKey(s => s.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Employees)
            .WithOne(e => e.Store)
            .HasForeignKey(e => e.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Sales)
            .WithOne(sale => sale.Store)
            .HasForeignKey(sale => sale.StoreId)
            .OnDelete(DeleteBehavior.SetNull);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}
