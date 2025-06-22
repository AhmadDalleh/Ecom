using Ecom.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.HasData(new DeliveryMethod
            {
                Id = 1,
                DeliveryTime = "Only a week",
                Description = "The fas delivery in the world",
                Name = "DHL",
                Price = 15
            },
            new DeliveryMethod
            {
                Id = 2,
                DeliveryTime = "Only tow weeks",
                Description = "Keep your product save and secure",
                Name = "XXX",
                Price = 12
            }
            );
        }
    }
}
