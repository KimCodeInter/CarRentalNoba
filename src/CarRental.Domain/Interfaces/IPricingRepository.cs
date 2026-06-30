using CarRental.Domain;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Interfaces;

public interface IPricingRepository
{
    Task<PricingRateDomain?> GetCurrentAsync(CarType carType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PricingRateDomain>> GetAllCurrentAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PricingRateDomain rate, CancellationToken cancellationToken = default);
}
