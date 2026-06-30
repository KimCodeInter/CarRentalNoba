using CarRental.Domain;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly CarRentalDbContext _context;

    public VehicleRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleDomain?> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Vehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber, cancellationToken);
        return entity is null ? null : ToDomain(entity);
    }

    public async Task<IReadOnlyList<VehicleDomain>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var rentedVehicleIds = await _context.Rentals
            .Where(r => r.ReturnDateTime == null)
            .Select(r => r.VehicleId)
            .ToListAsync(cancellationToken);

        var entities = await _context.Vehicles
            .Where(v => !rentedVehicleIds.Contains(v.Id))
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task AddAsync(VehicleDomain vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(ToEntity(vehicle), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(VehicleDomain vehicle, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Vehicles.FindAsync([vehicle.Id], cancellationToken);
        if (entity is not null)
        {
            _context.Vehicles.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static VehicleDomain ToDomain(VehicleEntity entity) =>
        VehicleDomain.Load(entity.Id, entity.RegistrationNumber, entity.CarType);

    private static VehicleEntity ToEntity(VehicleDomain domain) =>
        new() { Id = domain.Id, RegistrationNumber = domain.RegistrationNumber, CarType = domain.CarType };
}
