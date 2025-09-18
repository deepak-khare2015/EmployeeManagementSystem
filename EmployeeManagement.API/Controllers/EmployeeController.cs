using AutoMapper;
using DAL.Entities;
using EmployeeDBFirst_Library.Repositories;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interface;
using EmployeeManagement.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeeController : ControllerBase
    {
        #region Commented 
        //Week 1
        //private readonly AppDbContext _appDbContext;
        //private readonly IEmployeeRepository _employeeRepository;

        //public EmployeeController(IEmployeeRepository employeeRepository ,AppDbContext appDbContext)
        //{
        //    _employeeRepository = employeeRepository;
        //    _appDbContext = appDbContext;
        //}


        //Week 2
        //Unit Of Work
        //private readonly IUnitOfWork _uow;
        //public EmployeeController(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //}

        #region WEEK 3 COMMENT - TILL SERVICE LAYER

        //// GET: api/employees
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        //{
        //    #region Commented
        //    //week 1 repository pattern
        //    //var emp = await _appDbContext.Employees.ToListAsync();
        //    //var emp = await _employeeRepository.GetAllAsync();

        //    //UOW
        //    //var emp = await _uow.Employees.GetAllAsync();//Unit Of Work
        //    #endregion

        //    //week 3 service layer
        //    var emp = await _employeeService.GetAllAsync();
        //    return Ok(emp);
        //}

        //// GET: api/employees/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<Employee>>> GetById(int id)
        //{
        //    if (id == 0)
        //        return NotFound();

        //    #region Commented
        //    //Week 1 repository pattern
        //    //var emp = await _appDbContext.Employees.FindAsync(id);
        //    //var emp = await _employeeRepository.GetByIdAsync(id);

        //    //week 2 UOW
        //    //var emp = await _uow.Employees.GetByIdAsync(id); //Unit Of Work
        //    #endregion

        //    //Week 3 Service layer
        //    var emp = await _employeeService.GetByIdAsync(id);
        //    if (emp == null)
        //        return NotFound();

        //    return Ok(emp);

        //}

        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<Employee>>> Create(Employee emp)
        //{

        //    if (emp == null)
        //        return BadRequest();

        //    #region Commented
        //    //Week 1 Repsository pattern
        //    //_appDbContext.Add(emp);
        //    // await _employeeRepository.AddAsync(emp);
        //    //await _appDbContext.SaveChangesAsync();

        //    // Week 2 Unit Of Work
        //    // await _uow.Employees.AddAsync(emp);
        //    // await _uow.SaveAsync();
        //    #endregion


        //    //Week 3 Service layer
        //    await _employeeService.CreateAsync(emp);
        //    return CreatedAtAction(nameof(GetById), new { id = emp.Id }, emp);


        //}
        //[HttpPut("{id}")]
        //public async Task<ActionResult<IEnumerable<Employee>>> Update(int id, Employee employee)
        //{
        //    if (id == 0)
        //        return NotFound();

        //    #region Commented
        //    //Week 1 Repsository pattern
        //    // var emp = await _appDbContext.Employees.FindAsync(id);

        //    //Week 2 UOW
        //    //var emp = await _uow.Employees.GetByIdAsync(id);
        //    //if (emp == null)
        //    //    return NotFound();

        //    //emp.Salary = employee.Salary;
        //    //emp.Name = employee.Name;
        //    //emp.Age = employee.Age;

        //    //await _uow.Employees.UpdateAsync(emp);
        //    //await _uow.SaveAsync();
        //    #endregion

        //    //Week 3 Service Layer
        //    var emp = await _employeeService.UpdateAsync(employee, id);
        //    if (emp == null)
        //        return NotFound();

        //    return NoContent();
        //}


        ////[HttpDelete("{id}")]
        ////public async Task<ActionResult<IEnumerable<Employee>>> Delete(int id)
        ////{
        ////    if (id == 0)
        ////        return NotFound();

        ////    // var result = await _appDbContext.Employees.FindAsync(id);
        ////    var result = await _uow.Employees.GetByIdAsync(id); 

        ////    if (result == null)
        ////        return NotFound();

        ////    _appDbContext.Remove(result);
        ////    await _appDbContext.SaveChangesAsync();
        ////    return NoContent();
        ////}

        ////Unit Of Work

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<IEnumerable<Employee>>> Delete(int id)
        //{
        //    if (id == 0)
        //        return NotFound();

        //    #region Commented
        //    //Week 2 UOW
        //    //await _uow.Employees.DeleteAsyncs(id);
        //    //await _uow.SaveAsync();
        //    #endregion

        //    await _employeeService.DeleteAsync(id);
        //    return NoContent();
        //}

        #endregion

        #endregion

        private readonly IEmployeeService _employeeService;

        //Auto Mapper
        private readonly IMapper _mapper;

        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger )
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }


        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
        {
           
                //week 3 service layer
                var emp = await _employeeService.GetAllAsync();
                if(emp ==null)
                    return NotFound("Employee Not Found");

                //Auto Mapper Entity -> DTO
                var employeeDTO = _mapper.Map<IEnumerable<EmployeeDTO>>(emp);

                return Ok(employeeDTO);
           
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            _logger.LogInformation("Getting employee with id {Id}", id);

            if (id <= 0)
            {
                 _logger.LogError("Invalid id {Id} supplied", id);
                throw new ArgumentException("Id must be greater than 0");
            }


            // Error: log if not found
            if (id == 5) // 👈 pretend this id does not exist in DB
            {
                _logger.LogError("Employee with id {Id} not found", id);
                return NotFound("Employee not found");
            }





            //week 5 - employee service throws not found eception -> Handled by  custome exception middleware
            //Week 3 Service layer
            var emp = await _employeeService.GetByIdAsync(id);

                //Auto Mapper Entity-> DTO
                var employeeDTO = _mapper.Map<EmployeeDTO>(emp);

            _logger.LogInformation("Employee with id {Id} found successfully", id);

            return Ok(employeeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Create(EmployeeCreateDTO employeeCreateDTO)
        {

            //Auto Mapper DTO -> Enitty
            var employee = _mapper.Map<Employee>(employeeCreateDTO);

            // Week 5 employee service will throw notfound exception->handled by custome exception middleware
            //Week 3 Service layer
            await _employeeService.CreateAsync(employee);

            //Auto Mapper Entity -> DTO
            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

            return CreatedAtAction(nameof(GetById), new { id = employeeDTO.Id }, employeeDTO);


        }
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDTO>> Update(int id, EmployeeUpdateDTO employeeUpdateDTO)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0");

            //Week 5 employee service will throw notfound exception -> handled by custome exception middleware
            //Week 3 Service Layer
            var updateEmployee = await _employeeService.UpdateAsync(employeeUpdateDTO, id);
           
            //Auto Mapper Entity -> DTO
            var employeeDTO = _mapper.Map<EmployeeDTO>(updateEmployee);

            return Ok(employeeDTO);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0");

            //Week 5 employee service will throw notfound exception -> handled by custome exception middleware
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
