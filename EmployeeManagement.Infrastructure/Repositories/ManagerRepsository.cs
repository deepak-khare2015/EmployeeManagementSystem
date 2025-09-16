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
    public class ManagerRepsository : GenericRepository<Manager>, IManagerRepsository
    {
        public ManagerRepsository(AppDbContext context) : base(context)
        {
        }

        public async Task<Manager?> GetManagersByName(string name)
        {
            return await _dbset.FirstOrDefaultAsync(m => m.Name == name);
        }
    }
}
