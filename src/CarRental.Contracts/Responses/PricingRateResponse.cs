using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Responses;

public record PricingRateResponse(
    CarType CarType,
    decimal BaseDayRental,
    decimal BaseKmPrice,
    DateTimeOffset EffectiveFrom);
