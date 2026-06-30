using CarRental.Api.Middleware;
using CarRental.Application.Configuration;
using CarRental.Application.Handlers;
using CarRental.Domain;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Extensions;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<VehiclePricingOptions>(builder.Configuration.GetSection(VehiclePricingOptions.SectionName));

builder.Services.AddScoped<CreateVehicleHandler>();
builder.Services.AddScoped<GetAvailableVehiclesHandler>();
builder.Services.AddScoped<RemoveVehicleHandler>();
builder.Services.AddScoped<PickupVehicleHandler>();
builder.Services.AddScoped<ReturnVehicleHandler>();
builder.Services.AddScoped<GetRentedVehicleHandler>();
builder.Services.AddScoped<GetPriceEstimateHandler>();
builder.Services.AddScoped<GetActiveRentalsHandler>();
builder.Services.AddScoped<GetRentalsByCustomerHandler>();
builder.Services.AddScoped<GetVehicleRentalHistoryHandler>();
builder.Services.AddScoped<SetPricingHandler>();
builder.Services.AddScoped<GetCurrentPricingHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();
    db.Database.Migrate();
    await SeedDefaultPricingAsync(scope.ServiceProvider);
}

async Task SeedDefaultPricingAsync(IServiceProvider services)
{
    var pricingRepo = services.GetRequiredService<IPricingRepository>();
    var options = services.GetRequiredService<IOptions<VehiclePricingOptions>>().Value;

    var defaults = new[]
    {
        (CarType.Small, options.Small),
        (CarType.Combi, options.Combi),
        (CarType.Truck, options.Truck),
    };

    foreach (var (carType, pricing) in defaults)
    {
        if (await pricingRepo.GetCurrentAsync(carType) is null)
            await pricingRepo.AddAsync(PricingRateDomain.Set(carType, pricing.BaseDayRental, pricing.BaseKmPrice, DateTimeOffset.UtcNow));
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference(options => options.WithTitle("Car Rental API"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
