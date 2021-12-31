using CompanyEmployees.Domain.Models;
using CompanyEmployees.Service.Extensions.Utilities;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace CompanyEmployees.Service.Extensions
{
    public static class EmployeeRepositoryExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
            => employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm)
            || e.Position.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Name); 
            
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery)) 
                return employees.OrderBy(e => e.Name); 
            return employees.OrderBy(orderQuery);
        }
    }
}
