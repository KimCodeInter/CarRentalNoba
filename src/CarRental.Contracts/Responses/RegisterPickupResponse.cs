using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Responses;

public record RegisterPickupResponse(
    Guid Id,
    string BookingNumber,
    string RegistrationNumber,
    CarType CarType,
    DateTimeOffset PickupDateTime,
    int PickupMeterReading);
