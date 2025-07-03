using FastTechFoods.MenuService.Application.Commands.CreateProduct;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateProductHandler>();
});
// Add services to the container.
var app = builder.Build();


app.UseHttpsRedirection();
app.Run();

