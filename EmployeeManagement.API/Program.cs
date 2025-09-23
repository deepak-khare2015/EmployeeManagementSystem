using EmployeeDBFirst_Library;
using EmployeeManagement.API.MIddleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection; //Needed fo assembly
using System.IO;  //Needed for path
using Microsoft.OpenApi.Models; //Needed for OpenApiInfo



namespace EmployeeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Serilog Setup

            // --------------Serilog Setup----------------

            // Configure Serilog before app.Build() so ALL logs (even startup errors) go through Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)   // ⬅️ Reads from appsettings.json "Serilog" section
                .Enrich.FromLogContext()                         // ⬅️ Adds CorrelationId, RequestId, etc.
                .CreateLogger();                                 // ⬅️ Finalize logger

            // Load correct config automatically (Development/Production)
            // Replace built-in .NET logging with Serilog
            // Use Serilog with environment-aware config if you want to switch (prod -> dev) without changing appsettings.json
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration)
                              .ReadFrom.Services(services)
                              .Enrich.FromLogContext();

            });
            #endregion 

            // Add services to the container.
            builder.Services.AddControllers();

            //-------------Add Dependency Injection file-------------------------
            var connectionString = builder.Configuration.GetConnectionString("DbConnection");
            builder.Services.AddInfrastructure(connectionString);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();

            #region Swagger with XML comments
            //-------------Swagger with XML comments-------------------------
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Employee Management API",
                    Description = "An ASP.NET Core Web API for managing Employees",

                });


                var xmlfiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var xmlfile in xmlfiles)
                {
                    options.IncludeXmlComments(xmlfile);
                }
            });

            #endregion
            var app = builder.Build();



                //-----------------------------USE EXCEPTION MIDDLEWARE-----------------------
                app.UseMiddleware<ExceptionMiddleware>();


                //-----------------------------USE REQUEST RESPONSE LOGGING MIDDLEWARE-----------------------
                app.UseMiddleware<RequestResponseLoggingMiddleware>();

                //Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }


                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
    }
    }
