namespace EmployeeApi.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory;
        protected static HttpClient Client;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Init(TestContext testContext)
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();
        }

        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void Cleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

}
