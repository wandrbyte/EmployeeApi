namespace EmployeeApi.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected CustomWebApplicationFactory Factory;
        protected HttpClient Client;

        [TestInitialize]
        public void ClassInit()
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();
        }

        [TestCleanup]
        public  void ClassCleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

}
