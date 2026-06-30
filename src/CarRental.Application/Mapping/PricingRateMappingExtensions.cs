using CarRental.Contracts.Responses;
using CarRental.Domain;

namespace CarRental.Application.Mapping;

public static class PricingRateMappingExtensions
{
    public static PricingRateResponse ToResponse(this PricingRateDomain rate) =>
        new(rate.CarType.ToContract(), rate.BaseDayRental, rate.BaseKmPrice, rate.EffectiveFrom);
}
