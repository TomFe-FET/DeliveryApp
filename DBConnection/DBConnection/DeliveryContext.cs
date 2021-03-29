using DBConnection.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnection
{
    public class DeliveryContext : DbContext
    {
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {
        }
        public DbSet<OrderHead> OrderHead { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderHead>().ToTable("OrderHead");
            modelBuilder.Entity<OrderLine>().ToTable("OrderLines");

        }
    }
}
