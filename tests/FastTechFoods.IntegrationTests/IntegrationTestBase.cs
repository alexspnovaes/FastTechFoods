using System.Net.Http.Headers;
using FastTechFoods.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.IntegrationTests
{
    public abstract class IntegrationTestBase<TProgram, TContext>
        where TProgram : class
        where TContext : DbContext
    {
        protected readonly HttpClient Client;
        protected readonly Guid TestClientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        protected IntegrationTestBase()
        {
            var factory = new GenericWebApplicationFactory<TProgram, TContext>();
            Client = factory.CreateClient();
            var token = TestTokenGenerator.GenerateJwtToken(TestClientId, "client");
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
