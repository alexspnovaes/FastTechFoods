using FastTechFoods.BuildingBlocks.Messaging.AzureServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.BuildingBlocks.Security;
using FastTechFoods.KitchenService.API.Consumers;
using FastTechFoods.KitchenService.Application.Commands.AcceptOrder;
using FastTechFoods.KitchenService.Domain.Interfaces;
using FastTechFoods.KitchenService.Infrastructure.Data;
using FastTechFoods.KitchenService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddOpenTelemetry()
    .WithMetrics(mb =>
    {
        mb.AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation();
    })
    .WithTracing(tb =>
    {
        tb.AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation();
    });

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AcceptOrderHandler>();
});

builder.Services.AddJwtAuthentication(builder.Configuration);

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<KitchenDbContext>(opt =>
        opt.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<KitchenDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
}

var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
builder.Services.AddSingleton<IMessageBus>(
    _ => new AzureServiceBusMessageBus(connectionString));

builder.Services.AddAuthorization();

builder.Services.AddScoped<IKitchenOrderRepository, KitchenOrderRepository>();
builder.Services.AddHostedService<OrderCreatedConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Kitchen API", Version = "v1" });
});

var app = builder.Build();

app.UseHttpMetrics();
app.MapMetrics();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "text/plain";
        await ctx.Response.WriteAsync(report.Status.ToString());
    }
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "text/plain";
        await ctx.Response.WriteAsync(report.Status.ToString());
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

public partial class Program { }
