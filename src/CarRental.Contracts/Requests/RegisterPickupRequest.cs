namespace CarRental.Contracts.Requests;

public record RegisterPickupRequest(
    string RegistrationNumber,
    string CustomerSocialSecurityNumber,
    DateTimeOffset PickupDateTime,
    int PickupMeterReading);
