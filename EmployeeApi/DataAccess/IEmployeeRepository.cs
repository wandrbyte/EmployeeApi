using EmployeeApi.Models;

namespace EmployeeApi.DataAccess
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll(int top, int skip);
        Task<Employee?> GetById(int id);
        Task<Employee> Create(EmployeeRequest employeeRequest);
        Task Update(EmployeeRequest employeeRequest, Employee employee);
        Task Delete(Employee employee);
    }
}
