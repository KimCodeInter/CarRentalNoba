using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Requests;

public record SetPricingRequest(
    CarType CarType,
    decimal BaseDayRental,
    decimal BaseKmPrice);
