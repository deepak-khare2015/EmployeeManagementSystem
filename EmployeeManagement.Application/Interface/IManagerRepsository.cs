using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interface
{
    public  interface IManagerRepsository :IGenericRepository<Manager>
    {
        Task<Manager?> GetManagersByName(string name);


    }
}
