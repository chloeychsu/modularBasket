using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
         builder.HasKey(e=>e.Id);
         builder.HasIndex(e=>e.UserName);
         builder.Property(e=>e.UserName).IsRequired().HasMaxLength(100);
         builder.HasMany(e=>e.Items).WithOne().HasForeignKey(e=>e.ShoppingCartId);
    }
}
