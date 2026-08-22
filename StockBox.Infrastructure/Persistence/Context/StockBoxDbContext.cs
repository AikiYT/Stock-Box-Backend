using Microsoft.EntityFrameworkCore;
using Stock_Box.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Infrastructure.Persistence.Context
{
    public class StockBoxDbContext : DbContext
    {
        public StockBoxDbContext(DbContextOptions<StockBoxDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerServiceRecord> CustomerServiceRecords { get; set; }

        public DbSet<Debt> Debts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerServiceRecord>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.ServiceRecords)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Customer)
                .WithMany(c => c.Debts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}