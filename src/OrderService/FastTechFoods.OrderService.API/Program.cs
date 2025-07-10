using FastTechFoods.BuildingBlocks.Messaging.AzureServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.BuildingBlocks.Security;
using FastTechFoods.OrderService.Application.Commands.CreateOrder;
using FastTechFoods.OrderService.Domain.Interfaces;
using FastTechFoods.OrderService.Infrastructure.Data;
using FastTechFoods.OrderService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderHandler>();
});

builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
builder.Services.AddSingleton<IMessageBus>(
    _ => new AzureServiceBusMessageBus(connectionString));

// Add Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();


// Add Controllers
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
app.UseAuthorization();
app.MapControllers();
app.Run();
