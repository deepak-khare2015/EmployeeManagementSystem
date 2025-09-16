
using DAL.Entities;
using EmployeeDBFirst_Library;
using EmployeeDBFirst_Library.Repositories;
using EmployeeManagement.API.MIddleware;
using EmployeeManagement.Application.Interface;
using Microsoft.EntityFrameworkCore;


namespace EmployeeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("DbConnection");

            //Add Dependency Injection file
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

            //Exception middleware
            app.UseMiddleware<ExceptionMiddleware>(); 

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
