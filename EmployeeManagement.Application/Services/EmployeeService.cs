using AutoMapper;
using DAL.Entities;                                     // Employee entity
using EmployeeManagement.Application.DTOs;              // DTOs for Employee updates
using EmployeeManagement.Application.Exceptions;        // Custom exceptions
using EmployeeManagement.Application.Interface;         // IEmployeeService, IUnitOfWork
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
            var employees = await _uow.Employees.GetAllAsync();
            return employees;
        }

        // ---------------------- GET BY ID ----------------------
        public async Task<Employee?> GetByIdAsync(int id)
        {
            var employee = await _uow.Employees.GetByIdAsync(id);
            if (employee == null)
                throw new NotFoundException($"Employee with id {id} not found");

            return employee;
        }

        // ---------------------- CREATE ----------------------
        public async Task<Employee> CreateAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Employee cannot be null");

            await _uow.Employees.AddAsync(employee);   // Add employee using repository
            await _uow.SaveAsync();                    // Commit changes to DB
            return employee;
        }

        // ---------------------- DELETE ----------------------
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid employee id");

            var employee = await _uow.Employees.GetByIdAsync(id);
            if (employee == null)
                throw new NotFoundException($"Employee with id {id} not found");

            await _uow.Employees.DeleteAsyncs(id);     // Delete employee by id
            await _uow.SaveAsync();                    // Commit changes
            return true;
        }

        // ---------------------- UPDATE ----------------------
        public async Task<Employee> UpdateAsync(EmployeeUpdateDTO employeeUpdateDTO, int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid employee id");

            var existing = await _uow.Employees.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Employee with id {id} not found");

            // Use AutoMapper to update entity from DTO
            _mapper.Map(employeeUpdateDTO, existing);

            await _uow.Employees.UpdateAsync(existing); // Save updated entity
            await _uow.SaveAsync();                     // Commit transaction

            return existing;
        }
    }
}
