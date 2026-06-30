using CarRental.Domain.Enums;

namespace CarRental.Infrastructure.Persistence.Entities;

public class VehicleEntity
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; } = default!;
    public CarType CarType { get; set; }
}
