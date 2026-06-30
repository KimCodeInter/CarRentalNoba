namespace CarRental.Contracts.Responses;

public record RegisterReturnResponse(
    Guid Id,
    string BookingNumber,
    DateTimeOffset PickupDateTime,
    DateTimeOffset ReturnDateTime,
    int PickupMeterReading,
    int ReturnMeterReading,
    int NumberOfDays,
    int NumberOfKm,
    decimal TotalPrice);
