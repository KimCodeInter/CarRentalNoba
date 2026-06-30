using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRental.Infrastructure.Persistence;

public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }

    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();
    public DbSet<RentalEntity> Rentals => Set<RentalEntity>();
    public DbSet<PricingRateEntity> PricingRates => Set<PricingRateEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarRentalDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetToStringConverter>();
        configurationBuilder.Properties<DateTimeOffset?>()
            .HaveConversion<DateTimeOffsetToStringConverter>();
    }
}
