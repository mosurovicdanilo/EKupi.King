using EKupi.Application.Interfaces;
using EKupi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IAppDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductRelationship> ProductRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(orderDetails => new { orderDetails.OrderId, orderDetails.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasOne(orderDetails => orderDetails.Order)
                .WithMany(order => order.OrderDetails)
                .HasForeignKey(orderDetails => orderDetails.OrderId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(orderDetails => orderDetails.Product)
                .WithMany(product => product.OrderDetails)
                .HasForeignKey(orderDetails => orderDetails.ProductId);

            modelBuilder.Entity<OrderDetail>().Property(orderDetails => orderDetails.Price).HasColumnType("decimal").HasPrecision(2);
            modelBuilder.Entity<OrderDetail>().Property(orderDetails => orderDetails.Total).HasColumnType("decimal").HasPrecision(2);
            modelBuilder.Entity<Product>().Property(orderDetails => orderDetails.UnitPrice).HasColumnType("decimal").HasPrecision(2);

            modelBuilder.Entity<ProductRelationship>()
                .HasKey(productRelationship => productRelationship.Id);
            modelBuilder.Entity<ProductRelationship>()
                .HasOne(productRelationship => productRelationship.Product)
                .WithMany(product => product.ProductRelationship)
                .HasForeignKey(productRelationship => productRelationship.ProductId);
            modelBuilder.Entity<ProductRelationship>()
                .HasOne(productRelationship => productRelationship.RelatedProduct)
                .WithMany(product => product.RelatedProductRelationship)
                .HasForeignKey(productRelationship => productRelationship.RelatedProductId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)));
        }
    }
}
