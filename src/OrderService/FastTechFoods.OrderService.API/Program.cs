using FastTechFoods.OrderService.Application.Commands.CreateOrder;
using FastTechFoods.OrderService.Domain.Interfaces;
using FastTechFoods.OrderService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderHandler>();
});

// Add DbContext (InMemory por enquanto)
//builder.Services.AddDbContext<OrderDbContext>(opt =>
//    opt.UseInMemoryDatabase("OrderDb"));

// Add Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

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

app.UseAuthorization();
app.MapControllers();
app.Run();
