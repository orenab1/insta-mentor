using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Diagnostics;

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
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",optional: true)
                .Build();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            Log.Logger =
                new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(Configuration)
                    .CreateLogger();

            // var configuration =
            //     new ConfigurationBuilder()
            //         .AddJsonFile("appsettings.json",
            //         optional: true,
            //         reloadOnChange: true)
            //         .Build();

            // try
            // {
            //     //Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            //     Log.Logger =
            //         new LoggerConfiguration()
            //             .MinimumLevel
            //             .Debug()
            //             .MinimumLevel
            //             .Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            //             .Enrich
            //             .FromLogContext()
            //             .WriteTo
            //             .MSSqlServer("Server=DESKTOP-HJ7FJUC; database=vidcallme; Integrated Security=true;",
            //             "Logs")
            //             .CreateLogger();

            //     Log.Information("Hello");
            // }
            // catch (Exception ex)
            // {
            //     throw;
            // }

            // Log.Logger =
            //     new LoggerConfiguration()
            //         .MinimumLevel
            //         .Debug()
            //         .WriteTo
            //         .Console()
            //         .WriteTo
            //         .File("logs/myapp.txt",
            //         rollingInterval: RollingInterval.Day)
            //         // .WriteTo
            //         // .MSSqlServer(
            //         //     connectionString: "Server=DESKTOP-HJ7FJUC; database=vidcallme; Integrated Security=true;",
            //         //     sinkOptions: new MSSqlServerSinkOptions {
            //         //         TableName = "Logs"
            //         //     })
            //         .CreateLogger();

            // Log.Error("Hello, world!");
            // Log.CloseAndFlush();
            // int
            //     a = 10,
            //     b = 0;
            // try
            // {
            //     Log.Debug("Dividing {A} by {B}", a, b);
            //     Console.WriteLine(a / b);
            // }
            // catch (Exception ex)
            // {
            //     Log.Error(ex, "Something went wrong");
            // }
            // finally
            // {
            //     //Log.CloseAndFlush();
            // }

            // var columnOptionsSection = configuration.GetSection("Serilog:ColumnOptions");
            // var sinkOptionsSection = configuration.GetSection("Serilog:SinkOptions");

            // Legacy interace - do not use this anymore
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.MSSqlServer(
            //        connectionString: _connectionStringName,
            //        tableName: _tableName,
            //        appConfiguration: configuration,
            //        autoCreateSqlTable: true,
            //        columnOptionsSection: columnOptionsSection,
            //        schemaName: _schemaName)
            //    .CreateLogger();

            // // New SinkOptions based interface
            // Log.Logger = new LoggerConfiguration()
            //     .WriteTo.MSSqlServer(
            //         connectionString: _connectionStringName,
            //         sinkOptions: new MSSqlServerSinkOptions
            //         {
            //             TableName = _tableName,
            //             SchemaName = _schemaName,
            //             AutoCreateSqlTable = true
            //         },
            //         sinkOptionsSection: sinkOptionsSection,
            //         appConfiguration: configuration,
            //         columnOptionsSection: columnOptionsSection)
            //     .CreateLogger();

            // Log.Information("Hello {Name} from thread {ThreadId}", Environment.GetEnvironmentVariable("USERNAME"), Thread.CurrentThread.ManagedThreadId);

            // Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

            // Log.CloseAndFlush();
        }

        // public static IConfiguration Configuration
        // { get;
        // } =
        //     new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json",
        //         optional: false,
        //         reloadOnChange: true)
        //         .AddEnvironmentVariables()
        //         .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)    
               // .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
