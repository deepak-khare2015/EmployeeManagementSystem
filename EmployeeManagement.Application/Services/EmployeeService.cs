using AutoMapper;
using DAL.Entities;                                     // Employee entity
using EmployeeManagement.Application.DTOs;              // DTOs for Employee updates
using EmployeeManagement.Application.Exceptions;        // Custom exceptions
using EmployeeManagement.Application.Interface;         // IEmployeeService, IUnitOfWork
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    /// <summary>
    /// EmployeeService handles business logic for Employee CRUD operations.
    /// Uses UnitOfWork for repository access and AutoMapper for DTO ↔ Entity mapping.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;   // Unit of Work to manage repositories & save changes
        private readonly IMapper _mapper;    // AutoMapper for mapping DTOs to Entities

        public EmployeeService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // ---------------------- GET ALL ----------------------
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            Log.Information("Fetching all employees from database");  // ✅ log start
            var employees = await _uow.Employees.GetAllAsync();
            Log.Information("Fetched {Count} employees", employees?.Count() ?? 0);  // ✅ structured log
            return employees;
        }

        // ---------------------- GET BY ID ----------------------
        public async Task<Employee?> GetByIdAsync(int id)
        {
            Log.Information("Fetching employee with id {Id}", id);
            var employee = await _uow.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                Log.Warning("Employee with id {Id} not found", id);  // ✅ log warning
                throw new NotFoundException($"Employee with id {id} not found");
            }
            Log.Information("Employee with id {Id} fetched successfully", id);
            return employee;
        }

        // ---------------------- CREATE ----------------------
        public async Task<Employee> CreateAsync(Employee employee)
        {
            if (employee == null)
            {
                Log.Error("Attempted to create employee but input was null");  // ✅ log error
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null");
            }
            await _uow.Employees.AddAsync(employee);   // Add employee using repository
            await _uow.SaveAsync();                    // Commit changes to DB
            Log.Information("Employee created successfully with id {Id}", employee.Id);
            return employee;
        }

        // ---------------------- DELETE ----------------------
        public async Task<bool> DeleteAsync(int id)
        {
            Log.Information("Attempting to delete employee with id {Id}", id);

            if (id <= 0)
            {
                Log.Warning("Invalid employee id {Id} for delete", id);
                throw new ArgumentException("Invalid employee id");
            }

            var employee = await _uow.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                Log.Warning("Employee with id {Id} not found for delete", id);
                throw new NotFoundException($"Employee with id {id} not found");
            }
            await _uow.Employees.DeleteAsyncs(id);     // Delete employee by id
            await _uow.SaveAsync();                    // Commit changes
            Log.Information("Employee with id {Id} deleted successfully", id);
            return true;
        }

        // ---------------------- UPDATE ----------------------
        public async Task<Employee> UpdateAsync(EmployeeUpdateDTO employeeUpdateDTO, int id)
        {
            if (id <= 0)
            {
                Log.Warning("Invalid employee id {Id} for update", id);
                throw new ArgumentException("Invalid employee id");
            }
            var existing = await _uow.Employees.GetByIdAsync(id);
            if (existing == null)
            {
                Log.Warning("Employee with id {Id} not found for update", id);
                throw new NotFoundException($"Employee with id {id} not found");
            }
            // Use AutoMapper to update entity from DTO
            _mapper.Map(employeeUpdateDTO, existing);

            await _uow.Employees.UpdateAsync(existing); // Save updated entity
            await _uow.SaveAsync();                     // Commit transaction
            Log.Information("Employee with id {Id} updated successfully", id);
            return existing;
        }
    }
}
