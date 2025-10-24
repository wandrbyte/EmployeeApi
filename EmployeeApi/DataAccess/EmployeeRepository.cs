using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.DataAccess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Employee>> GetAll(int top, int skip)
        {
            return await _context.Employees.Skip(skip).Take(top).ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> Create(EmployeeRequest employeeRequest)
        {
            var employee = new Employee
            {
                Name = employeeRequest.Name,
                DateOfBirth = employeeRequest.DateOfBirth,
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task Update(EmployeeRequest employeeRequest, Employee employee)
        {
            employee.Name = employeeRequest.Name;
            employee.DateOfBirth = employeeRequest.DateOfBirth;

            await _context.SaveChangesAsync();
        }



        public async Task Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
