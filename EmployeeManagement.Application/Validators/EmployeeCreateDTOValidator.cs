using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTOs;
using FluentValidation;


namespace EmployeeManagement.Application.Validators
{
    public class EmployeeCreateDTOValidator : AbstractValidator<EmployeeCreateDTO>
    {
        public EmployeeCreateDTOValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Name is Reqired")
                .MaximumLength(20).WithMessage("Name can not exceed 20 characters");


            RuleFor(e => e.Age)
                .InclusiveBetween(18, 50).WithMessage("Age must be between 18 nd 50");

            RuleFor(e => e.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0");

        }
    }
}
