namespace EmployeeApi.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory;
        protected static HttpClient Client;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInit(TestContext context)
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

}
