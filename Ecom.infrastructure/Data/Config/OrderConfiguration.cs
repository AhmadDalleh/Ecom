using Ecom.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress,
                n => { n.WithOwner(); });

            builder.HasMany(x=>x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Status).HasConversion(o => o.ToString(),
                o => (Status)Enum.Parse(typeof(Status), o));

            builder.Property(m => m.SubTotal).HasColumnType("decimal(18,2)");
        }
    }

}
