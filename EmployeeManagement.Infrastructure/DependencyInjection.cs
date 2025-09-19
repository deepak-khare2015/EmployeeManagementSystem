using DAL.Entities;
using EmployeeDBFirst_Library.Repositories;
using EmployeeManagement.Application.Interface;
using EmployeeManagement.Application.Mapping;
using EmployeeManagement.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDBFirst_Library
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure (this IServiceCollection services, string ConnectionString)
        {
            //DbContext
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(ConnectionString)
            .EnableSensitiveDataLogging(false)); // If you want to ignore sensitive data in logs

            //Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IManagerRepsository, ManagerRepsository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            //Register Automapper
            services.AddAutoMapper(typeof(EmployeeProfile)); 

            return services;


        }
    }
}
