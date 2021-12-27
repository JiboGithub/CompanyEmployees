using AutoMapper;
using CompanyEmployees.Domain.Dtos;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return BadRequest("CompanyForCreationDto object is null");
            }
            var companyEntity = _mapper.Map<Company>(company);
            await _repository.Company.CreateCompany(companyEntity); 
            await _repository.SaveAsync(); 
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return CreatedAtRoute("CompanyById", 
                new { 
                    companyId = companyToReturn.Id }, 
                companyToReturn);
        }


        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _repository.Company.GetAllCompanies(trackChanges: false);
                var companiesDto =  _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return Ok(companiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCompanies)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
            }
        }


        [HttpGet("{companyId}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false); 
            if (company is null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database."); 
                return NotFound(); 
            } 
            else 
            {
                var companyDto = _mapper.Map<CompanyDto>(company); 
                return Ok(companyDto); 
            }
        }
    }
}
