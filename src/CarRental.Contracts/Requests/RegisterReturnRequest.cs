namespace CarRental.Contracts.Requests;

public record RegisterReturnRequest(
    string BookingNumber,
    DateTimeOffset ReturnDateTime,
    int ReturnMeterReading);
