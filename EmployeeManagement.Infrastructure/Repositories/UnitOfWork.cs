using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using EmployeeManagement.Application.Interface;

namespace EmployeeDBFirst_Library.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appContext;
        public IEmployeeRepository Employees { get; }
        public IManagerRepsository Managers { get; }

        public UnitOfWork(AppDbContext appContext, IEmployeeRepository employeeRepository, IManagerRepsository managerRepsository)
        {

            _appContext = appContext;
            Employees = employeeRepository;
            Managers = managerRepsository;
        }
        
        public async Task<int> SaveAsync()
        {
            return await _appContext.SaveChangesAsync();
        }

        public void Dispose()
        {

            _appContext.Dispose();
        }
    }
}   
