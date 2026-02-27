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

var corsPolicyName = "Bike2BeansUI";



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODAzNjg2NDAwIiwiaWF0IjoiMTc3MjIyMzYxOSIsImFjY291bnRfaWQiOiIwMTljYTBjMWNiNjU3N2VjOGE2Njk0NTA0MThmMTM0MiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2pnYzRybjJ0ZGN4OHFiOWp2eTZodjQ1Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.P9upNk-NjmRSh0aL-Q92aSJOpPCeNfXnV5zDtT-JYIl_HP_qS03ZHymkdUbK0A6XN-IxgzaFlljADqztHzE6oFMGhr_hADLIitw5cUkPlmy3K2XfGVRpYJpm_eSiWKNIgC9nqapyOfyRvt-ZYyMjcXTPL3BN2XbnCH9c9nB5z3ZFPQAnZMqOZFvlI0LUvinE2bUqNs-WAFsV_FWmC5DL0ndBi3xpTVDuFSJxe90UZweGBF-ETj7KKBYfeTXUKtgBVT0isa_-Kget2qD1lA4wJH0KVcyY0mkVZ8pNs2z-mRkih34f-ytY6cPVZYBV65sEQZjfyrew2roH4bmZVRjn8w";
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// MVC Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo Settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") // your webpack dev server
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // only if you're using cookies/auth
    });
});




builder.Services.AddGooglePlaces(builder.Configuration);
builder.Services.AddMapbox(builder.Configuration);

// Services
builder.Services.AddScoped<CoffeeShopRepository>();
builder.Services.AddScoped<RouteRepository>();
builder.Services.AddScoped<GetAllCoffeeShopHandler>();
builder.Services.AddScoped<GetRouteDetailsByIdHandler>();
builder.Services.AddScoped<CreateCoffeeShopHandler>();
builder.Services.AddScoped<SearchNearbyCoffeeShopHandler>();
builder.Services.AddScoped<SearchCoffeeShopByIdHandler>();
builder.Services.AddScoped<SearchCoffeeShopByTextHandler>();
builder.Services.AddScoped<AutocompleteHandler>();
builder.Services.AddScoped<GetAllRouteDetailsHandler>();
builder.Services.AddScoped<CreateRouteDetailsHandler>();
builder.Services.AddScoped<CreateRouteHandler>();



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
