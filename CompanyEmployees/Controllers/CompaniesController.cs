using AutoMapper;
using CompanyEmployees.Domain.Dtos;
using CompanyEmployees.Domain.ModelBinders;
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id) 
        {
            var company = await _repository.Company.GetCompany(id, trackChanges: false);
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database."); 
                return NotFound(); 
            }
            await _repository.Company.DeleteCompany(company); 
            await _repository.SaveAsync();
            return NoContent(); 
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


        [HttpPost("collection")] 
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection) 
        {
            if (companyCollection == null)
            { 
                _logger.LogError("Company collection sent from client is null."); 
                return BadRequest("Company collection is null"); 
            }
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection); 
            foreach (var company in companyEntities) 
            {
                await _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync(); 
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities); 
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id)); 
            return CreatedAtRoute("CompanyCollection", 
                new { 
                    ids }, companyCollectionToReturn
                    );
        }



        [HttpGet("collection/({ids})", Name = "CompanyCollection")] 
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) 
            {
                _logger.LogError("Parameter ids is null"); 
                return BadRequest("Parameter ids is null"); 
            } 
            var companyEntities = await _repository.Company.GetCompaniesByIds(ids, trackChanges: false); 
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection"); 
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities); 
            return Ok(companiesToReturn);
        }
    }
}
