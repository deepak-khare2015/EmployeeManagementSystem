using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs
{
    public class EmployeeCreateDTO
    {
        //[Required(ErrorMessage = "Name is required")]
        //[StringLength(20,ErrorMessage = "Name can not exceed 20 characters")]
        public string Name { get; set; }

       
       // [Range(18,50,ErrorMessage ="Age must be between 18 and 50")]
        public int Age { get; set; }

       
       //` [Range(1, double.MaxValue,ErrorMessage ="Salary must be grater than 0")]
        public decimal Salary { get; set; }

    }
}
