﻿using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.DataAccess.GenericRepository.Service;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Service.Interfaces;
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

        public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges) 
            => await FindByConditionAsync(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).Result.SingleOrDefaultAsync();

        public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges)
            => await Task.Run(() => FindByConditionAsync(e => e.CompanyId.Equals(companyId), trackChanges).Result.OrderBy(e => e.Name));
    }
}
