using AutoMapper;
using DAL.Entities;
using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Mapping
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            //Read Operation
            //Entities -> DTO
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Salary, opt => opt.Ignore());// Ignore Salary for output DTO

            //Write Operation
            //DTO -> Entities
            CreateMap<EmployeeCreateDTO, Employee>();
            CreateMap<EmployeeUpdateDTO, Employee>();


        }
    }
}
