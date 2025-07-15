using FastTechFoods.BuildingBlocks.Messaging.AzureServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;
using FastTechFoods.BuildingBlocks.Security;
using FastTechFoods.KitchenService.API.Consumers;
using FastTechFoods.KitchenService.Application.Commands.AcceptOrder;
using FastTechFoods.KitchenService.Domain.Interfaces;
using FastTechFoods.KitchenService.Infrastructure.Data;
using FastTechFoods.KitchenService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
public partial class Program { }

