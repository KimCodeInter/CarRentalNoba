using CarRental.Contracts.Responses;
using CarRental.Domain;

namespace CarRental.Application.Mapping;

public static class VehicleMappingExtensions
{
    public static VehicleResponse ToVehicleResponse(this VehicleDomain vehicle) =>
        new(vehicle.Id, vehicle.RegistrationNumber, vehicle.CarType.ToContract());
}
