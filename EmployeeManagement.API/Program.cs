using EmployeeDBFirst_Library;
using EmployeeManagement.API.MIddleware;
using EmployeeManagement.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; //Needed for OpenApiInfo
using Serilog;
using System.IO;  //Needed for path
using System.Reflection; //Needed fo assembly



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

            //builder.Services.AddControllers();

            //Fluent validation custom error response format middleware
            builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Intercept validation before ASP.NET Core writes ProblemDetails
        options.InvalidModelStateResponseFactory = context =>
        {
            // Collect validation errors into a list
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new
                {
                    field = e.Key,
                    messages = e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                }).ToList();

            // Save errors in HttpContext so our ValidationMiddleware can handle it
            context.HttpContext.Items["ModelStateErrors"] = errors;

            // Return bare 400 with NO BODY — this prevents ProblemDetails from being written
            return new EmptyResult(); // ⚡ instead of BadRequestResult
        };
    });

           

            //Fluent validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateDTOValidator>();




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

            //-----------------------------USE VALIDATOR MIDDLEWARE TO ADD FLUENT VALIDATION LOG IN GENERIC CUSTOME FORMAT-----------------------
            app.UseMiddleware<ValidationMiddleware>();

           

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
