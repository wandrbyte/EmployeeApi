using EmployeeApi.DataAccess;
using EmployeeApi.Models;
using EmployeeApi.Validators;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeValidator _employeeValidator;

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator)
        {
            ArgumentNullException.ThrowIfNull(employeeRepository);
            ArgumentNullException.ThrowIfNull(employeeValidator);
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(int top = 50, int skip = 0)
        {
            if (top < 0 || top > 100) top = 50;
            if (skip < 0) skip = 0;

            var employees = await _employeeRepository.GetAll(top, skip);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            return employee is null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeRequest employeeRequest)
        {
            if (!_employeeValidator.IsValidEmployee(employeeRequest))
            {
                return BadRequest("Employee can't be younger than 18");
            }

            var employee = await _employeeRepository.Create(employeeRequest);
            
            return CreatedAtAction(nameof(GetEmployee), new { Id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeRequest employeeRequest)
        {
            var employee = await _employeeRepository.GetById(id);
            if(employee is null) return NotFound();

            if (!_employeeValidator.IsValidEmployee(employeeRequest))
            {
                return BadRequest("Employee can't be younger than 18");
            }

            await _employeeRepository.Update(employeeRequest, employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetById(id);
            if (employee == null)
                return NotFound();

            await _employeeRepository.Delete(employee);

            return NoContent();
        }

    }
}
