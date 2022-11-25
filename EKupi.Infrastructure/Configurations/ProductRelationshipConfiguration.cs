using EKupi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace EKupi.Infrastructure.Configurations
{
    public class ProductRelationshipConfiguration : IEntityTypeConfiguration<ProductRelationship>
    {
        public void Configure(EntityTypeBuilder<ProductRelationship> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.RelatedProduct)
                .WithMany(p => p.SubProductOf)
                .HasForeignKey(p => p.RelatedProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.SubProducts)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
