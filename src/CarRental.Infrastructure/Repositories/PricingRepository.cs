using CarRental.Domain;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

public class PricingRepository : IPricingRepository
{
    private readonly CarRentalDbContext _context;

    public PricingRepository(CarRentalDbContext context)
    {
        _context = context;
    }

    public async Task<PricingRateDomain?> GetCurrentAsync(CarType carType, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PricingRates
            .Where(p => p.CarType == carType)
            .OrderByDescending(p => p.EffectiveFrom)
            .FirstOrDefaultAsync(cancellationToken);
        return entity is null ? null : ToDomain(entity);
    }

    public async Task<IReadOnlyList<PricingRateDomain>> GetAllCurrentAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<PricingRateDomain>();
        foreach (var carType in Enum.GetValues<CarType>())
        {
            var rate = await GetCurrentAsync(carType, cancellationToken);
            if (rate is not null)
                result.Add(rate);
        }
        return result;
    }

    public async Task AddAsync(PricingRateDomain rate, CancellationToken cancellationToken = default)
    {
        await _context.PricingRates.AddAsync(ToEntity(rate), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static PricingRateDomain ToDomain(PricingRateEntity entity) =>
        PricingRateDomain.Load(entity.Id, entity.CarType, entity.BaseDayRental, entity.BaseKmPrice, entity.EffectiveFrom);

    private static PricingRateEntity ToEntity(PricingRateDomain domain) =>
        new() { Id = domain.Id, CarType = domain.CarType, BaseDayRental = domain.BaseDayRental, BaseKmPrice = domain.BaseKmPrice, EffectiveFrom = domain.EffectiveFrom };
}
