using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Domain.Services;

public static class RentalPriceCalculator
{
    private const decimal CombiDayRateMultiplier = 1.3m;
    private const decimal TruckDayRateMultiplier = 1.5m;
    private const decimal TruckKmRateMultiplier = 1.5m;

    public static decimal Calculate(CarType carType, int numberOfDays, int numberOfKm, decimal baseDayRental, decimal baseKmPrice) =>
        carType switch
        {
            CarType.Small => baseDayRental * numberOfDays,
            CarType.Combi => baseDayRental * numberOfDays * CombiDayRateMultiplier + baseKmPrice * numberOfKm,
            CarType.Truck => baseDayRental * numberOfDays * TruckDayRateMultiplier + baseKmPrice * numberOfKm * TruckKmRateMultiplier,
            _ => throw new DomainException($"No price formula defined for type '{carType}'.")
        };
}
