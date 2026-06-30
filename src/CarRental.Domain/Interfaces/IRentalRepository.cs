using CarRental.Domain;

namespace CarRental.Domain.Interfaces;

public interface IRentalRepository
{
    Task<RentalDomain?> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RentalDomain>> GetByCustomerSsnAsync(string customerSsn, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RentalDomain>> GetByVehicleRegistrationAsync(string registrationNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RentalDomain>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(RentalDomain rental, CancellationToken cancellationToken = default);
    Task UpdateAsync(RentalDomain rental, CancellationToken cancellationToken = default);
    Task<bool> HasActiveRentalAsync(Guid vehicleId, CancellationToken cancellationToken = default);
}
