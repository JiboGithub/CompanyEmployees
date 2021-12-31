using AutoMapper;
using CompanyEmployees.Domain.Dtos;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Domain.RequestFeatures;
using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.Service.Filters.ActionFilters;
using CompanyEmployees.Service.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")] 
    //[ApiController] 
    public class EmployeesController : ControllerBase 
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
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
                new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn);
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id) 
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;
            await _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync(); 
            return NoContent(); 
        }


        [HttpGet]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
            {
                 return BadRequest("Max age can't be less than min age.");
            }

            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb); 
            return Ok(employeesDto); 
        }



        [HttpGet("{employeeId}", Name = "GetEmployeeForCompany")] 
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId) 
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false); 
            if (company is null) 
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
            return Ok(employee);
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee) 
        {
            var employeeEntity = HttpContext.Items["employee"] as Employee;
            _mapper.Map(employee, employeeEntity); 
            await _repository.SaveAsync(); 
            return NoContent(); 
        }


        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null."); 
                return BadRequest("patchDoc object is null"); 
            }
            var employeeEntity = HttpContext.Items["employee"] as Employee;
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity); 
            patchDoc.ApplyTo(employeeToPatch, ModelState);

            TryValidateModel(employeeToPatch);

            if (!ModelState.IsValid) 
            {
                _logger.LogError("Invalid model state for the patch document"); 
                return UnprocessableEntity(ModelState); 
            }

            _mapper.Map(employeeToPatch, employeeEntity); 
            await _repository.SaveAsync(); 
            return NoContent(); 
        }
    }
}
