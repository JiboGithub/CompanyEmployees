using CompanyEmployees.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges);
        Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        Task CreateEmployeeForCompany(Guid companyId, Employee employee);
    }
}
