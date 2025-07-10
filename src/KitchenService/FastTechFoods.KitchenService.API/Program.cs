using FastTechFoods.BuildingBlocks.Messaging.AzureServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.KitchenService.API.Consumers;
using FastTechFoods.KitchenService.Application.Commands.AcceptOrder;
using FastTechFoods.KitchenService.Domain.Interfaces;
using FastTechFoods.KitchenService.Infrastructure.Data;
using FastTechFoods.KitchenService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<AcceptOrderHandler>();
});

builder.Services.AddDbContext<KitchenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
builder.Services.AddSingleton<IMessageBus>(
    _ => new AzureServiceBusMessageBus(connectionString));

var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IKitchenOrderRepository, KitchenOrderRepository>();
builder.Services.AddHostedService<OrderCreatedConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();
app.Run();
