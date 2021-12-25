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
        public async Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges)
            => await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<Company> GetCompany(Guid companyId, bool trackChanges) 
            => await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

    }
}
