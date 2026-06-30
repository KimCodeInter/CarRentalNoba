using CarRental.Contracts.Enums;

namespace CarRental.Contracts.Requests;

public record CreateVehicleRequest(string RegistrationNumber, CarType CarType);
