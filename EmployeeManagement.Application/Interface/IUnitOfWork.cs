using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interface
{
    public  interface IUnitOfWork :IDisposable
    {

        IEmployeeRepository Employees {get;}
        IManagerRepsository Managers {get;}

        Task<int> SaveAsync();
    }
}
