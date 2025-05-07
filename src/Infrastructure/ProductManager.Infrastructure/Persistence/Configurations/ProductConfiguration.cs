using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManager.Domain.Entities;

namespace ProductManager.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(p => p.Description)
                .HasMaxLength(1000);
                
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
                
            builder.Property(p => p.CreatedAt)
                .IsRequired();
                
            builder.Property(p => p.UpdatedAt)
                .IsRequired(false);
                
            builder.HasIndex(p => p.Code)
                .IsUnique();
                
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.IsActive);
        }
    }
}
