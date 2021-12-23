using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.DataAccess.GenericRepository.Service;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CompanyEmployees.Service.Services
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
            => FindAll(trackChanges).OrderBy(c => c.Name).ToList();
    }
}
