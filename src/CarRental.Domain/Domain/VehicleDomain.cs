using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Domain;

public class VehicleDomain
{
    public Guid Id { get; private set; }
    public string RegistrationNumber { get; private set; }
    public CarType CarType { get; private set; }

    private VehicleDomain(Guid id, string registrationNumber, CarType carType)
    {
        Id = id;
        RegistrationNumber = registrationNumber;
        CarType = carType;
    }

    public static VehicleDomain Load(Guid id, string registrationNumber, CarType carType) =>
        new(id, registrationNumber, carType);

    public static VehicleDomain Create(string registrationNumber, CarType carType)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new DomainException("Registration number is required.");

        return new VehicleDomain(Guid.NewGuid(), registrationNumber, carType);
    }
}
