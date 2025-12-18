using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DepartmentName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.DepartmentCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Budget)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.EmployeeCount)
            .HasDefaultValue(0);

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(d => d.DepartmentCode)
            .IsUnique()
            .HasDatabaseName("IX_Departments_DepartmentCode");

        builder.HasIndex(d => d.DepartmentName)
            .HasDatabaseName("IX_Departments_DepartmentName");

        builder.HasIndex(d => d.ManagerId)
            .HasDatabaseName("IX_Departments_ManagerId");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Departments_IsActive");

        // Relationships
        builder.HasOne(d => d.Manager)
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
