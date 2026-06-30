using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Responses;

public record VehicleResponse(Guid Id, string RegistrationNumber, CarType CarType);
