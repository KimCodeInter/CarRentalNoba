using CarRental.Application.Mapping;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class SetPricingHandler
{
    private readonly IPricingRepository _repository;

    public SetPricingHandler(IPricingRepository repository)
    {
        _repository = repository;
    }

    public async Task<PricingRateResponse> HandleAsync(SetPricingRequest request, CancellationToken cancellationToken = default)
    {
        var rate = PricingRateDomain.Set(
            request.CarType.ToDomain(),
            request.BaseDayRental,
            request.BaseKmPrice,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(rate, cancellationToken);

        return rate.ToResponse();
    }
}
