using CarRental.Domain.Enums;

namespace CarRental.Infrastructure.Persistence.Entities;

public class PricingRateEntity
{
    public Guid Id { get; set; }
    public CarType CarType { get; set; }
    public decimal BaseDayRental { get; set; }
    public decimal BaseKmPrice { get; set; }
    public DateTimeOffset EffectiveFrom { get; set; }
}
