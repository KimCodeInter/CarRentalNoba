using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Responses;

public record PriceEstimateResponse(
    CarType CarType,
    decimal BaseDayRental,
    decimal BaseKmPrice,
    int NumberOfDays,
    int NumberOfKm,
    decimal EstimatedPrice);
