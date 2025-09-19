
using DAL.Entities;
using EmployeeDBFirst_Library;
using EmployeeDBFirst_Library.Repositories;
using EmployeeManagement.API.MIddleware;
using EmployeeManagement.Application.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace EmployeeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --------------Serilog Setup----------------

            // Configure Serilog before app.Build() so ALL logs (even startup errors) go through Serilog
            Log.Logger = new LoggerConfiguration()

                // 1️⃣ Read configuration from appsettings.json ("Serilog" section)
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()  // ✅ Adds context (RequestId, etc.)
                .CreateLogger();// 5️⃣ Finalize Serilog configuration


            // Replace built-in .NET logging with Serilog
            builder.Host.UseSerilog();


            // Add services to the container.
            builder.Services.AddControllers();

            //-------------Add Dependency Injection file-------------------------
            var connectionString = builder.Configuration.GetConnectionString("DbConnection");
            builder.Services.AddInfrastructure(connectionString);

            #region Moved into DependencyInjection.cs
            ////ConnectionString
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            //});

            ////Register dependencies
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //builder.Services.AddScoped<IManagerRepsository, ManagerRepsository>();

            ////Unit of Work
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //-----------------------------USE EXCEPTION MIDDLEWARE-----------------------
            app.UseMiddleware<ExceptionMiddleware>();


            //-----------------------------USE REQUEST RESPONSE LOGGING MIDDLEWARE-----------------------
            app.UseMiddleware<RequestResponseLoggingMiddleware>();  

            // Configure the HTTP request pipeline.
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
