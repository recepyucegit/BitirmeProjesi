using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Table
        builder.ToTable("Roles");

        // Primary Key
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.IsActive)
            .HasDefaultValue(true);

        builder.Property(r => r.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(r => r.Name)
            .IsUnique();

        builder.HasIndex(r => r.IsActive);

        // Seed Data - Default Roles
        builder.HasData(
            new Role { Id = 1, Name = "Admin", Description = "System Administrator", IsActive = true, CreatedDate = DateTime.Now },
            new Role { Id = 2, Name = "BranchManager", Description = "Şube Müdürü", IsActive = true, CreatedDate = DateTime.Now },
            new Role { Id = 3, Name = "Cashier", Description = "Kasa Satış", IsActive = true, CreatedDate = DateTime.Now },
            new Role { Id = 4, Name = "Warehouse", Description = "Depo Sorumlusu", IsActive = true, CreatedDate = DateTime.Now },
            new Role { Id = 5, Name = "Accounting", Description = "Muhasebe", IsActive = true, CreatedDate = DateTime.Now },
            new Role { Id = 6, Name = "TechnicalService", Description = "Teknik Servis", IsActive = true, CreatedDate = DateTime.Now }
        );

        // Query Filter - Soft Delete
        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
