using CarRental.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Tests.Integration;

public class CarRentalApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<CarRentalDbContext>));
            services.Remove(descriptor);

            services.AddDbContext<CarRentalDbContext>(options =>
                options.UseSqlite(_connection));
        });
    }

    public async Task InitializeAsync() => await _connection.OpenAsync();

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
