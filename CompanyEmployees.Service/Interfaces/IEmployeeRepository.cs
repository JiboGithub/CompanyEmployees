using CompanyEmployees.Domain.Models;
using CompanyEmployees.Domain.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyEmployees.Service.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        Task CreateEmployeeForCompany(Guid companyId, Employee employee);
        Task DeleteEmployee(Employee employee);
    }
}
