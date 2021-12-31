using CompanyEmployees.LoggerService.Interfaces;
using CompanyEmployees.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Filters.ActionFilters
{
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository; 
        private readonly ILoggerManager _logger; 

        public ValidateCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger) 
        {
            _repository = repository; _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) 
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");

            var id = (Guid)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("companyId")).SingleOrDefault()];

            var company = await _repository.Company.GetCompany(id, trackChanges); 
            if (company is null) 
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database."); 
                context.Result = new NotFoundResult(); 
            }
            else 
            { context.HttpContext.Items.Add("company", company); 
                await next(); 
            }
        }
    }
}
