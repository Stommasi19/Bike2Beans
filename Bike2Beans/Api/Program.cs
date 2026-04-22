using System.Threading.RateLimiting;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Api.Configuration;
using Bike2Beans.Application.Mapper;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using Bike2Beans.Infrastructure.Extension;
using Bike2Beans.Infrastructure.Gateways;
using Bike2Beans.Infrastructure.Repositories;
using Microsoft.AspNetCore.RateLimiting;

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

// auth
builder.Services.AddFirebaseAuthentication(builder.Configuration);

builder.Services.Configure<ApiCostGuardOptions>(
    builder.Configuration.GetSection(ApiCostGuardOptions.SectionName)
);

var apiCostGuards = builder.Configuration
    .GetSection(ApiCostGuardOptions.SectionName)
    .Get<ApiCostGuardOptions>() ?? new ApiCostGuardOptions();

builder.Services.AddRateLimiter(options =>
{
    static FixedWindowRateLimiterOptions CreatePolicy(int permitLimit) => new()
    {
        PermitLimit = Math.Max(permitLimit, 1),
        Window = TimeSpan.FromMinutes(1),
        QueueLimit = 0,
        AutoReplenishment = true
    };

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { error = "Rate limit exceeded. Slow down and try again." },
            cancellationToken: token
        );
    };

    options.AddPolicy(ApiRateLimitPolicies.PlacesAutocomplete, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            ApiRateLimitPolicies.BuildPartitionKey(httpContext, ApiRateLimitPolicies.PlacesAutocomplete),
            _ => CreatePolicy(apiCostGuards.AutocompleteRequestsPerMinute)
        ));

    options.AddPolicy(ApiRateLimitPolicies.PlacesSearch, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            ApiRateLimitPolicies.BuildPartitionKey(httpContext, ApiRateLimitPolicies.PlacesSearch),
            _ => CreatePolicy(apiCostGuards.PlaceSearchRequestsPerMinute)
        ));

    options.AddPolicy(ApiRateLimitPolicies.RouteGeneration, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            ApiRateLimitPolicies.BuildPartitionKey(httpContext, ApiRateLimitPolicies.RouteGeneration),
            _ => CreatePolicy(apiCostGuards.RouteGenerationRequestsPerMinute)
        ));
});

builder.Services.AddGooglePlaces(builder.Configuration);
builder.Services.AddMapbox(builder.Configuration);
builder.Services.AddFirebaseAdmin(builder.Configuration);
builder.Services.AddMongo(builder.Configuration);

// // Services
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<ICoffeeshopRepository, CoffeeshopRepository>();
builder.Services.AddScoped<IRouteProvider, MapboxRestGateway>();
// builder.Services.AddScoped<ILocationProvider, GooglePlacesRestGateway>();
builder.Services.AddScoped<IMapper<Coffeeshop, CoffeeshopDto>, CoffeeshopMapper>();
builder.Services.AddScoped<IMapper<RouteOption, RouteOptionDto>, RouteOptionMapper>();
builder.Services.AddScoped<IMapper<RouteStop, RouteStopDto>, RouteStopMapper>();
builder.Services.AddScoped<IMapper<User, UserDto>, UserMapper>();
builder.Services.AddScoped<IUserBootstrapRepository, UserBootstrapRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();
