using DBConnection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;


[assembly: WebJobsStartup(typeof(AzureFunction.FunctionStartup))]
namespace AzureFunction
{
    public class FunctionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {

            var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();
            builder.Services.AddDbContext<DeliveryContext>(_config =>
            {
                _config.UseSqlServer(config.GetConnectionString("SqlConnectionString"));
            });
        }

    }
}