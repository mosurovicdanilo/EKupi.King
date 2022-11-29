using EKupi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Hrana"
                },
                new Category
                {
                    Id = 2,
                    Name = "Piće"
                },
                new Category
                {
                    Id = 3,
                    Name = "Elektronika"
                },
                new Category
                {
                    Id = 4,
                    Name = "Materijal"
                },
                new Category
                {
                    Id = 5,
                    Name = "Auto-moto"
                }
            );
        }
    }
}
