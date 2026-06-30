using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=carrental.db";

        services.AddDbContext<CarRentalDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IPricingRepository, PricingRepository>();

        return services;
    }
}
