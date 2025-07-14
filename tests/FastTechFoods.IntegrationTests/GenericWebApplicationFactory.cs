using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FastTechFoods.IntegrationTests
{
    public class GenericWebApplicationFactory<TProgram, TContext>
        : WebApplicationFactory<TProgram>
        where TProgram : class
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
        }
    }
}
