using CompanyEmployees.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompanies(bool trackChanges);
        Task<Company> GetCompany(Guid companyId, bool trackChanges);
        Task CreateCompany(Company company);
        Task<IEnumerable<Company>> GetCompaniesByIds(IEnumerable<Guid> ids, bool trackChanges);
        Task DeleteCompany(Company company);
    }
}