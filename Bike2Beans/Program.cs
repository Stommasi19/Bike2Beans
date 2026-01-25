using Bike2Beans.Data;
using Bike2Beans.Application.CoffeeShops.Queries.GetAll;
using Bike2Beans.Application.CoffeeShops.Commands.Create;



var builder = WebApplication.CreateBuilder(args);

// MVC Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo Settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Services
builder.Services.AddScoped<CoffeeShopRepository>();
builder.Services.AddScoped<GetAllCoffeeShopHandler>();
builder.Services.AddScoped<CreateCoffeeShopHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
