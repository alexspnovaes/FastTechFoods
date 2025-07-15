using FastTechFoods.AuthService.Application.Services;
using FastTechFoods.AuthService.Domain.Entities;
using FastTechFoods.AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace AuthService.IntegrationTests;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        var customized = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var tokenDesc = services
                    .Single(d => d.ServiceType == typeof(ITokenService));
                if (tokenDesc != null) services.Remove(tokenDesc);
                services.AddSingleton<ITokenService, FakeTokenService>();
            });
        });

        _client = customized.CreateClient();
    }

    [Fact]
    public async Task Register_ThenLogin_ReturnsFakeToken()
    {
        var registerPayload = new
        {
            Name = "Test User",
            Email = "test@example.com",
            CPF = (string?)null,
            Password = "Pwd@123",
            Role = "client"
        };

        var regResp = await _client.PostAsJsonAsync("/api/auth/register", registerPayload);
        regResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginPayload = new
        {
            EmailOrCpf = "test@example.com",
            Password = "Pwd@123"
        };

        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);
        loginResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var doc = await loginResp.Content.ReadFromJsonAsync<JsonDocument>();
        var token = doc.RootElement.GetProperty("token").GetString();
        token.Should().Be("fake-token");
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var loginPayload = new
        {
            EmailOrCpf = "wrong@example.com",
            Password = "badpwd"
        };

        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);
        loginResp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private class FakeTokenService : ITokenService
    {
        public string GenerateToken(User user) => "fake-token";
    }
}