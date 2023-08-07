using AlphaOneA.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Pomelo.EntityFrameworkCore;

namespace AlphaOneA
{
        public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var watcher = serviceProvider.GetRequiredService<FileWatcherService>();
            watcher.Start();

            // Keep the application running
            Console.ReadLine();


        }
        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
                
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .WriteTo.File(path: @"C:\temp\Log\Log.txt",
                                             rollingInterval: RollingInterval.Minute,
                                             rollOnFileSizeLimit: true,
                                             fileSizeLimitBytes: 2000)
                             .Enrich.FromLogContext()
                             .CreateLogger();

            var serviceProvider = new ServiceCollection()
                            .AddLogging(builder => builder.AddSerilog())
                            .AddSingleton<IConfiguration>(configuration)
                            .AddScoped<FileWatcherService>()
                            .AddScoped<SqliteDatabaseService>()
                            .AddDbContext<AppDbContext>(options =>
             {
                 options.UseMySql("Server=localhost;Database=mydb;User=root;Password=admin;",
                     ServerVersion.AutoDetect("Server=localhost;Database=mydb;User=root;Password=admin;"));            
             }
             )
                .BuildServiceProvider();

            return serviceProvider;
        }

    }
}