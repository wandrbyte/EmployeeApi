using EmployeeApi.Models;
using System.Net;
using System.Net.Http.Json;

namespace EmployeeApi.IntegrationTests
{
    [TestClass]
    public sealed class EmployeeApiTests : IntegrationTestBase
    {
        private readonly DateOnly _defaultDob = DateOnly.FromDateTime(DateTime.Today).AddYears(-20);

        [TestMethod]
        public async Task GetEmployeeById_ReturnsNull_WhenEmployeeDoesntExist()
        {
            var response = await Client.GetAsync("/api/employees/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public async Task GetEmployeeById_ReturnsOk_WhenEmployeeExists()
        {
            var emp = await CreateEmployeeAsync("John");
            var response = await Client.GetFromJsonAsync<Employee>($"/api/employees/{emp.Id}");
            Assert.IsNotNull(response);
            Assert.AreEqual("John", response.Name);
            Assert.AreEqual(emp.Id, response.Id);
            Assert.AreEqual(_defaultDob, response.DateOfBirth);

        }

        [TestMethod]
        public async Task GetEmployees_ReturnsOk()
        {
            await CreateEmployeeAsync();

            var response = await Client.GetFromJsonAsync<List<Employee>>("api/employees");

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count == 0); //breaking tests to check for CI
        }

        [TestMethod]
        public async Task CreateEmployee_ReturnsBadRequest_WhenAgeInvalid()
        {
            var request = new EmployeeRequest
            {
                Name = "Test",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today)
            };

            var response = await Client.PostAsJsonAsync<EmployeeRequest>("api/employees", request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateEmployee_ReturnsCreated_WhenAgeValid()
        {
            var request = new EmployeeRequest
            {
                Name = "Test",
                DateOfBirth = _defaultDob
            };

            var response = await Client.PostAsJsonAsync<EmployeeRequest>("api/employees", request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsBadRequest_WhenAgeInvalid()
        {
            var emp = await CreateEmployeeAsync();
            var request = new EmployeeRequest
            {
                Name = emp.Name,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today)
            };

            var response = await Client.PutAsJsonAsync<EmployeeRequest>($"api/employees/{emp.Id}", request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmpDoesntExist()
        {
            var request = new EmployeeRequest
            {
                Name = "test",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today)
            };

            var response = await Client.PutAsJsonAsync<EmployeeRequest>($"api/employees/{12345}", request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task UpateEmployee_ReturnsNoContent_WhenAgeValid()
        {
            var emp = await CreateEmployeeAsync();
            var request = new EmployeeRequest
            {
                Name = emp.Name,
                DateOfBirth = _defaultDob.AddYears(-5)
            };

            var response = await Client.PutAsJsonAsync<EmployeeRequest>($"api/employees/{emp.Id}", request);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsNotFound_WhenEmpDoesntExist()
        {
            var response = await Client.DeleteAsync($"api/employees/{12345}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteEmployee_ReturnsNoContent_WhenEmployeeExist()
        {
            var emp = await CreateEmployeeAsync();
            var request = new EmployeeRequest
            {
                Name = emp.Name,
                DateOfBirth = _defaultDob.AddYears(-5)
            };

            var response = await Client.DeleteAsync($"api/employees/{emp.Id}");
            var getResponse = await Client.GetAsync($"api/employee/{emp.Id}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        private async Task<Employee> CreateEmployeeAsync(string name = "TestUser")
        {
            var emp = new Employee { Name = name, DateOfBirth = _defaultDob };
            var response = await Client.PostAsJsonAsync("/api/employees", emp);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Employee>();
        }

    }
}
