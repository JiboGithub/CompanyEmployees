using CompanyEmployees.DataAccess.Data;
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
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateCompany(Company company) => await CreateAsync(company);

        public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges)
            => await FindAllAsync(trackChanges).Result.OrderBy(c => c.Name).ToListAsync();

        public async Task<Company> GetCompany(Guid companyId, bool trackChanges) 
            => await FindByConditionAsync(c => c.Id.Equals(companyId), trackChanges).Result.SingleOrDefaultAsync();

    }
}
