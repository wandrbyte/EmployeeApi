using EmployeeApi.Models;

namespace EmployeeApi.Validators
{
    public interface IEmployeeValidator
    {
        public bool IsValidEmployee(EmployeeRequest employee);
    }
}
