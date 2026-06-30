using CarRental.Domain;

namespace CarRental.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<VehicleDomain?> GetByRegistrationNumberAsync(string registrationNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VehicleDomain>> GetAvailableAsync(CancellationToken cancellationToken = default);
    Task AddAsync(VehicleDomain vehicle, CancellationToken cancellationToken = default);
    Task RemoveAsync(VehicleDomain vehicle, CancellationToken cancellationToken = default);
}
