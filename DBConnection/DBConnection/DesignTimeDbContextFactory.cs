using DBConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBConnection
{
    // Connection String wird definiert, läuft passiv ab
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeliveryContext>
    {
        public DeliveryContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DeliveryContext>();
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            builder.UseSqlServer(connectionString);
            return new DeliveryContext(builder.Options);
        }
    }
}
