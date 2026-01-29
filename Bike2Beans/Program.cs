using Bike2Beans.Data;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using Google.Api.Gax.Grpc;
using Google.Api.Gax.Grpc.Rest;
using Google.Maps.Places.V1;




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

// Services
builder.Services.AddScoped<CoffeeShopRepository>();
builder.Services.AddScoped<GetAllCoffeeShopHandler>();
builder.Services.AddScoped<CreateCoffeeShopHandler>();
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = (config["GooglePlaces:ApiKey"] ?? "").Trim();

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new InvalidOperationException("Missing GooglePlaces:ApiKey in configuration.");

    return new PlacesClientBuilder
    {
        ApiKey = apiKey,
        GrpcAdapter = RestGrpcAdapter.Default
    }.Build();
});
builder.Services.AddScoped<SearchNearbyCoffeeShopHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
