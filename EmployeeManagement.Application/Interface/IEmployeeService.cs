using DAL.Entities;
using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync (Employee employee);

        Task<Employee> UpdateAsync(EmployeeUpdateDTO employeeUpdateDTO , int id);
        Task<bool> DeleteAsync(int id);

        
       
       


    }
}
