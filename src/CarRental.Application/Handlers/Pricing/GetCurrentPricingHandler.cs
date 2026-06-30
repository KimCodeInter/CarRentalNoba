using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetCurrentPricingHandler
{
    private readonly IPricingRepository _repository;

    public GetCurrentPricingHandler(IPricingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PricingRateResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var rates = await _repository.GetAllCurrentAsync(cancellationToken);
        return rates.Select(r => r.ToResponse()).ToList();
    }
}
