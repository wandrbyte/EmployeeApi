using EmployeeApi.Models;

namespace EmployeeApi.Validators
{
    public class EmployeeValidator : IEmployeeValidator
    {
        private const int _minValidAge = 18;

        public bool IsValidEmployee(EmployeeRequest employee)
        {
            return IsValidAge(employee.DateOfBirth);
        }

        private static bool IsValidAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age >= _minValidAge;
        }
    }
}
