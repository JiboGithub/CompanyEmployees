using System.Threading.Tasks;

namespace CompanyEmployees.Service.Interfaces
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        Task SaveAsync();
    }
}
