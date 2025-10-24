using EmployeeApi.Models;
using EmployeeApi.Validators;

namespace EmployeeApi.UnitTests
{
    [TestClass]
    public sealed class EmployeeValidatorTests
    {
        [TestMethod]
        public void IsValidEmployee_ReturnsTrue_WhenAgeValid()
        {
            var employeeRequest = new EmployeeRequest
            {
                Name = "Test",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today).AddYears(-18)
            };

            var validator = new EmployeeValidator();
            var result = validator.IsValidEmployee(employeeRequest);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmployee_ReturnsFalse_WhenAgeInvalid()
        {
            var invalidDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-10);

            var employeeRequest = new EmployeeRequest
            {
                Name = "Test",
                DateOfBirth = invalidDate
            };

            var validator = new EmployeeValidator();
            var result = validator.IsValidEmployee(employeeRequest);
            Assert.IsFalse(result);
        }
    }
}
