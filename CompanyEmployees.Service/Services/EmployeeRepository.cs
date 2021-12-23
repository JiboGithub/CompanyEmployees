using CompanyEmployees.DataAccess.Data;
using CompanyEmployees.DataAccess.GenericRepository.Service;
using CompanyEmployees.Domain.Models;
using CompanyEmployees.Service.Interfaces;

namespace CompanyEmployees.Service.Services
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
