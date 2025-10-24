namespace EmployeeApi.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected CustomWebApplicationFactory Factory;
        protected HttpClient Client;

        [TestInitialize]
        public void Init()
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

}
