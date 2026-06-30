using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Responses;

public record RentalDetailsResponse(
    Guid Id,
    string BookingNumber,
    string RegistrationNumber,
    string CustomerSocialSecurityNumber,
    CarType CarType,
    DateTimeOffset PickupDateTime,
    int PickupMeterReading,
    DateTimeOffset? ReturnDateTime,
    int? ReturnMeterReading,
    decimal? TotalPrice,
    bool IsReturned);
