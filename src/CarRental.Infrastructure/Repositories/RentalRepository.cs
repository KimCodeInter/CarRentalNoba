using CarRental.Domain;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly CarRentalDbContext _context;

    public RentalRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<RentalDomain?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Rentals.FirstOrDefaultAsync(r => r.BookingNumber == bookingNumber, cancellationToken);
        return entity is null ? null : ToDomain(entity);
    }

    public async Task<IReadOnlyList<RentalDomain>> GetByCustomerSsnAsync(string customerSsn, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Rentals
            .Where(r => r.CustomerSocialSecurityNumber == customerSsn)
            .OrderByDescending(r => r.PickupDateTime)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task<IReadOnlyList<RentalDomain>> GetByVehicleRegistrationAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Rentals
            .Where(r => r.RegistrationNumber == registrationNumber)
            .OrderByDescending(r => r.PickupDateTime)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task<IReadOnlyList<RentalDomain>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Rentals
            .Where(r => r.ReturnDateTime == null)
            .OrderBy(r => r.PickupDateTime)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task AddAsync(RentalDomain rental, CancellationToken cancellationToken = default)
    {
        await _context.Rentals.AddAsync(ToEntity(rental), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RentalDomain rental, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Rentals.FindAsync([rental.Id], cancellationToken);
        if (entity is not null)
        {
            entity.ReturnDateTime = rental.ReturnDateTime;
            entity.ReturnMeterReading = rental.ReturnMeterReading;
            entity.TotalPrice = rental.TotalPrice;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public Task<bool> HasActiveRentalAsync(Guid vehicleId, CancellationToken cancellationToken = default)
        => _context.Rentals.AnyAsync(r => r.VehicleId == vehicleId && r.ReturnDateTime == null, cancellationToken);

    private static RentalDomain ToDomain(RentalEntity entity) =>
        RentalDomain.Load(
            entity.Id, entity.BookingNumber, entity.VehicleId, entity.RegistrationNumber,
            entity.CarType, entity.CustomerSocialSecurityNumber,
            entity.PickupDateTime, entity.PickupMeterReading,
            entity.BaseDayRental, entity.BaseKmPrice,
            entity.ReturnDateTime, entity.ReturnMeterReading, entity.TotalPrice);

    private static RentalEntity ToEntity(RentalDomain domain) =>
        new()
        {
            Id = domain.Id,
            BookingNumber = domain.BookingNumber,
            VehicleId = domain.VehicleId,
            RegistrationNumber = domain.RegistrationNumber,
            CarType = domain.CarType,
            CustomerSocialSecurityNumber = domain.CustomerSocialSecurityNumber,
            PickupDateTime = domain.PickupDateTime,
            PickupMeterReading = domain.PickupMeterReading,
            BaseDayRental = domain.BaseDayRental,
            BaseKmPrice = domain.BaseKmPrice,
            ReturnDateTime = domain.ReturnDateTime,
            ReturnMeterReading = domain.ReturnMeterReading,
            TotalPrice = domain.TotalPrice
        };
}
