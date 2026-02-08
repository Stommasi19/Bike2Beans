using Bike2Beans.Data;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using Bike2Beans.Application.CoffeeShops.Queries.Search;
using Bike2Beans.Application.CoffeeShops.Queries.Autocomplete;
using Google.Api.Gax.Grpc;
using Google.Api.Gax.Grpc.Rest;
using Google.Maps.Places.V1;
using Bike2Beans.Options;
using Microsoft.Extensions.Options;
using Bike2Beans.Infrastructure;




var builder = WebApplication.CreateBuilder(args);
// Console.WriteLine(builder.Configuration["Google:PlacesApiKey"] is null
//     ? "Missing Google Places key"
//     : "Google Places key loaded");


// MVC Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo Settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));




builder.Services.AddGooglePlaces(builder.Configuration);

// Services
builder.Services.AddScoped<CoffeeShopRepository>();
builder.Services.AddScoped<GetAllCoffeeShopHandler>();
builder.Services.AddScoped<CreateCoffeeShopHandler>();
builder.Services.AddScoped<SearchNearbyCoffeeShopHandler>();
builder.Services.AddScoped<SearchCoffeeShopByIdHandler>();
builder.Services.AddScoped<SearchCoffeeShopByTextHandler>();
builder.Services.AddScoped<AutocompleteHandler>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
