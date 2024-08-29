using Microsoft.AspNetCore.Mvc.Testing;

namespace Customers.Api.IntegrationTests
{
    [CollectionDefinition("CustomerApi Collection")]
    public class TestCollection : ICollectionFixture<WebApplicationFactory<ApiAssembly>>
    {
    }
}
