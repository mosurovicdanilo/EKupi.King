using EKupi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Infrastructure.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.Price).IsRequired();

            builder.Property(o => o.Quantity).IsRequired();

            builder.Property(o => o.Total).IsRequired();

            builder.HasOne(p => p.Product)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
