using Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Application.Mapper;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using Bike2Beans.Infrastructure.Extension;
using Bike2Beans.Infrastructure.Gateways;
using Bike2Beans.Infrastructure.Repositories;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFirebaseAdmin(builder.Configuration);
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
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<ICoffeeshopRepository, CoffeeshopRepository>();
builder.Services.AddScoped<IRouteProvider, MapboxRestGateway>();
// builder.Services.AddScoped<ILocationProvider, GooglePlacesRestGateway>();
builder.Services.AddScoped<IMapper<Coffeeshop, CoffeeshopDto>, CoffeeshopMapper>();
builder.Services.AddScoped<IMapper<RouteOption, RouteOptionDto>, RouteOptionMapper>();
builder.Services.AddScoped<IMapper<RouteStop, RouteStopDto>, RouteStopMapper>();

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
