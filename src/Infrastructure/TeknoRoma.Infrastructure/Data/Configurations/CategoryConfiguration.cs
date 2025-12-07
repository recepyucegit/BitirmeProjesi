using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table
        builder.ToTable("Categories");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("GETDATE()");

        // Indexes
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.IsActive);

        // Query Filter - Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);

        // Relationships
        builder.HasMany(e => e.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
