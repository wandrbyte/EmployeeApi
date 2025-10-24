using EmployeeApi.Controllers;
using EmployeeApi.DataAccess;
using EmployeeApi.Models;
using EmployeeApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeApi.UnitTests
{
    [TestClass]
    public sealed class EmployeeControllerTests
    {
        [TestMethod]
        public async Task GetEmployee_ReturnsNotFound_WhenMissing()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();
            mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Employee)null);

            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.GetEmployee(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetEmployee_ReturnsOk_WhenFound()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            var employee = new Employee
            {
                Id = 1,
                Name = "John",
                DateOfBirth = new DateOnly(2002, 11, 5)
            };
            mockRepo.Setup(r => r.GetById(employee.Id)).ReturnsAsync(employee);

            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.GetEmployee(1);

            var okResult = result as OkObjectResult;
            var employeeResult = okResult.Value as Employee;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(employeeResult);

            Assert.AreEqual(employee.Id, employeeResult.Id);
        }

        [TestMethod]
        public async Task GetEmployees_ReturnsOk()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            var employees = new List<Employee>();
            var employee = new Employee
            {
                Id = 1,
                Name = "Test",
                DateOfBirth = new DateOnly(2002, 11, 5)
            };
            employees.Add(employee);

            mockRepo.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(employees);

            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.GetEmployees();

            var okResult = result as OkObjectResult;
            var employeeResult = okResult.Value as List<Employee>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(employeeResult);
            Assert.AreEqual(employeeResult.Count, 1);
        }

        [TestMethod]
        public async Task CreateEmployee_ReturnsBadRequest_WhenValidatorFails()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            mockValidator.Setup(v => v.IsValidEmployee(It.IsAny<EmployeeRequest>())).Returns(false);
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.CreateEmployee(new EmployeeRequest { Name = "Test", DateOfBirth = DateOnly.MaxValue });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateEmployee_ReturnsCreated_WhenValidatorPasses()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();
            var employee = new Employee()
            {
                Id = 1,
                Name = "Test",
                DateOfBirth = DateOnly.MaxValue,
            };
            mockValidator.Setup(v => v.IsValidEmployee(It.IsAny<EmployeeRequest>())).Returns(true);
            mockRepo.Setup(r => r.Create(It.IsAny<EmployeeRequest>())).ReturnsAsync(employee);

            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.CreateEmployee(new EmployeeRequest
            {
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth
            });

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmployeeDoesNotExists()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();
            //mockRepo.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Employee)null);
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.UpdateEmployee(1, new EmployeeRequest { Name = "Test", DateOfBirth = DateOnly.MaxValue });

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenValidatorFails()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            mockValidator.Setup(v => v.IsValidEmployee(It.IsAny<EmployeeRequest>())).Returns(false);
            mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Employee { Id = 1, Name = "Test" });
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.UpdateEmployee(1, new EmployeeRequest { Name = "Test", DateOfBirth = DateOnly.MaxValue });

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsNoContent_WhenSuccess()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            mockValidator.Setup(v => v.IsValidEmployee(It.IsAny<EmployeeRequest>())).Returns(true);
            mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Employee { Id = 1, Name = "Test" });
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.UpdateEmployee(1, new EmployeeRequest { Name = "Test", DateOfBirth = DateOnly.MaxValue });

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsNotFound_WhenEmployeeDoesNotExists()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.DeleteEmployee(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsNoContent_WhenSuccess()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var mockValidator = new Mock<IEmployeeValidator>();

            mockValidator.Setup(v => v.IsValidEmployee(It.IsAny<EmployeeRequest>())).Returns(true);
            mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Employee { Id = 1, Name = "Test" });
            var controller = new EmployeesController(mockRepo.Object, mockValidator.Object);

            var result = await controller.DeleteEmployee(1);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

    }
}
