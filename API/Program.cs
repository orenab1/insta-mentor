using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Sinks.File;
using Serilog.Sinks.MSSqlServer;

namespace API
{
    public class Program
    {
        private const string _connectionStringName = "DefaultConnection";

        private const string _schemaName = "dbo";

        private const string _tableName = "LogEvents";

        public static IConfiguration Configuration
        { get;
        } =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                optional: false,
                reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                optional: true)
                .Build();

        public static void Main(string[] args)
        {
            Log.Logger =
                new LoggerConfiguration()
                    .WriteTo
                    .Console()
                    .WriteTo
                    .File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel
                    .Debug()
                    .CreateBootstrapLogger();


            Log.Information("Starting up!");

            try
            {
                CreateHostBuilder(args).Build().Run();

                Log.Information("Stopped cleanly");
                
            } 
            catch (Exception ex)
            {
                Log
                    .Fatal(ex,
                    "An unhandled exception occured during bootstrapping");
            }
            finally
            {
                Log.CloseAndFlush();
            }

           
        }
   
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) =>
                    configuration
                        .ReadFrom
                        .Configuration(context.Configuration)
                        .ReadFrom
                        .Services(services)
                        .Enrich
                        .FromLogContext()
                        .WriteTo
                        .Console()
                        .WriteTo
                        .File("logs/logs.txt",
                        rollingInterval: RollingInterval.Day)
                        .WriteTo
                        .MSSqlServer(
                            connectionString: "Server=localhost;Database=vidcallme;Integrated Security=SSPI;",
                            sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" })
                        // .MinimumLevel
                        // .Debug()
                        )
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
