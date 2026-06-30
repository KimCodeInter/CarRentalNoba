using CarRental.Domain.Enums;

namespace CarRental.Infrastructure.Persistence.Entities;

public class RentalEntity
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; } = default!;
    public Guid VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = default!;
    public CarType CarType { get; set; }
    public string CustomerSocialSecurityNumber { get; set; } = default!;
    public DateTimeOffset PickupDateTime { get; set; }
    public int PickupMeterReading { get; set; }
    public decimal BaseDayRental { get; set; }
    public decimal BaseKmPrice { get; set; }
    public DateTimeOffset? ReturnDateTime { get; set; }
    public int? ReturnMeterReading { get; set; }
    public decimal? TotalPrice { get; set; }
}
