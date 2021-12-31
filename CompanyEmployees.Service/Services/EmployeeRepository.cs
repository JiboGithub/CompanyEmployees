using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.DataAccess.GenericRepository.Service;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Domain.RequestFeatures;
using CompanyEmployees.Service.Interfaces;
using CompanyEmployees.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Services
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId; 
            await CreateAsync(employee);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            await RemoveAsync(employee);
        }

        public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) 
            => await FindByConditionAsync(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).Result.SingleOrDefaultAsync();

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) 
        {
            var employees = await FindByConditionAsync(e => e.CompanyId.Equals(companyId), trackChanges).Result
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize).ToListAsync(); 
            
            var count = await FindByConditionAsync(e => e.CompanyId.Equals(companyId), trackChanges).Result
                .CountAsync(); 
            
            return new PagedList<Employee>(employees, employeeParameters.PageNumber, employeeParameters.PageSize, count); 
        }
    }
}
