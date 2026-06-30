using CarRental.Domain.Exceptions;
using ContractType = CarRental.Contracts.Enums.CarType;
using DomainType = CarRental.Domain.Enums.CarType;

namespace CarRental.Application.Mapping;

public static class CarTypeMappingExtensions
{
    public static DomainType ToDomain(this ContractType carType) =>
        carType switch
        {
            ContractType.Small => DomainType.Small,
            ContractType.Combi => DomainType.Combi,
            ContractType.Truck => DomainType.Truck,
            _ => throw new DomainException($"Unknown car type: {carType}")
        };

    public static ContractType ToContract(this DomainType carType) =>
        carType switch
        {
            DomainType.Small => ContractType.Small,
            DomainType.Combi => ContractType.Combi,
            DomainType.Truck => ContractType.Truck,
            _ => throw new DomainException($"Unknown car type: {carType}")
        };
}
