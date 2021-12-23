using CompanyEmployees.Domain.Models;
using System.Collections.Generic;

namespace CompanyEmployees.Service.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
    }
}
