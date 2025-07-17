using FastTechFoods.BuildingBlocks.Security;
using FastTechFoods.MenuService.Application.Commands.CreateProduct;
using FastTechFoods.MenuService.Domain.Interfaces;
using FastTechFoods.MenuService.Infrastructure.Data;
using FastTechFoods.MenuService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddApplicationInsightsTelemetry();

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

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<MenuDbContext>(opt =>
        opt.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<MenuDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
}

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateProductHandler>());
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Menu API", Version = "v1" });
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
