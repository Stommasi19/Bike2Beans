using Bike2Beans.Application.CommandsAndQueries.Autocomplete;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Infrastructure.Extension;
using Bike2Beans.Infrastructure.Repositories;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetAllCoffeeshopHandler>();
});

// MVC Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo Settings
var corsPolicyName = "Bike2BeansUI";
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(MongoDBSettings.SectionName));
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});




builder.Services.AddGooglePlaces(builder.Configuration);
builder.Services.AddMapbox(builder.Configuration);

// // Services
builder.Services.AddScoped<CoffeeShopRepository>();
builder.Services.AddScoped<RouteRepository>();
// builder.Services.AddScoped<GetRouteDetailsByIdHandler>();
// builder.Services.AddScoped<SearchNearbyCoffeeShopHandler>();
// builder.Services.AddScoped<SearchCoffeeshopByIdHandler>();
// builder.Services.AddScoped<SearchCoffeeShopByTextHandler>();
// builder.Services.AddScoped<AutocompleteHandler>();
// builder.Services.AddScoped<GetAllRouteDetailsHandler>();
// builder.Services.AddScoped<CreateRouteDetailsHandler>();
// builder.Services.AddScoped<CreateRouteHandler>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();
