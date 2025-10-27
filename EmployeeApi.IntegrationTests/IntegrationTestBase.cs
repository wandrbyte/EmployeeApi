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

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass, ClassCleanupBehavior.EndOfClass)]
        public static void ClassCleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }

}
