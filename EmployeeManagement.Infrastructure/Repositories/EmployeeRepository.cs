using DAL.Entities;
using EmployeeManagement.Application.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDBFirst_Library.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

       
        public async Task<Employee?> GetEmployeeByName(string name)
        {
            return await _dbset.FirstOrDefaultAsync(e => e.Name == name); 
        }
    }
}
