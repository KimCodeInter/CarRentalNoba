using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;

namespace CarRental.Domain;

public class PricingRateDomain
{
    public Guid Id { get; private set; }
    public CarType CarType { get; private set; }
    public decimal BaseDayRental { get; private set; }
    public decimal BaseKmPrice { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }

    private PricingRateDomain(Guid id, CarType carType, decimal baseDayRental, decimal baseKmPrice, DateTimeOffset effectiveFrom)
    {
        Id = id;
        CarType = carType;
        BaseDayRental = baseDayRental;
        BaseKmPrice = baseKmPrice;
        EffectiveFrom = effectiveFrom;
    }

    public static PricingRateDomain Load(Guid id, CarType carType, decimal baseDayRental, decimal baseKmPrice, DateTimeOffset effectiveFrom) =>
        new(id, carType, baseDayRental, baseKmPrice, effectiveFrom);

    public static PricingRateDomain Set(CarType carType, decimal baseDayRental, decimal baseKmPrice, DateTimeOffset effectiveFrom)
    {
        if (baseDayRental <= 0)
            throw new DomainException("Base day rental must be greater than zero.");
        if (baseKmPrice < 0)
            throw new DomainException("Base km price cannot be negative.");

        return new PricingRateDomain(Guid.NewGuid(), carType, baseDayRental, baseKmPrice, effectiveFrom);
    }
}
