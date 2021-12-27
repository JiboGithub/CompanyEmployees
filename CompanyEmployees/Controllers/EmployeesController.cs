using AutoMapper;
using CompanyEmployees.Domain.Dtos;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")] 
    [ApiController] public class EmployeesController : ControllerBase 
    {
        private readonly IRepositoryManager _repository; 
        private readonly ILoggerManager _logger; 
        private readonly IMapper _mapper; 
        public EmployeesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) 
        {
            _repository = repository; 
            _logger = logger; 
            _mapper = mapper; 
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null) 
            {
                _logger.LogError("EmployeeForCreationDto object sent from client is null."); 
                return BadRequest("EmployeeForCreationDto object is null"); 
            }
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database."); 
                return NotFound();
            }
            var employeeEntity = _mapper.Map<Employee>(employee); 
            await _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity); 
            await _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity); 
            return CreatedAtRoute("GetEmployeeForCompany", 
                new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }


        [HttpGet] 
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId) 
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false); 
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database."); 
                return NotFound(); 
            } 
            var employeesFromDb = await _repository.Employee.GetEmployees(companyId, trackChanges: false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb); 
            return Ok(employeesDto); 
        }



        [HttpGet("{employeeId}", Name = "GetEmployeeForCompany")] 
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId) 
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false); 
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database."); 
                return NotFound();
            } 
            var employeeDb = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges: false); 
            if (employeeDb == null) 
            { 
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound(); 
            }
            var employee = _mapper.Map<EmployeeDto>(employeeDb); 
            return Ok(employee); }
    }
}
