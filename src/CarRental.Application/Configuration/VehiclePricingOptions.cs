namespace CarRental.Application.Configuration;

public class VehiclePricingOptions
{
    public const string SectionName = "VehiclePricing";

    public CarTypePricing Small { get; set; } = new() { BaseDayRental = 100m, BaseKmPrice = 0m };
    public CarTypePricing Combi { get; set; } = new() { BaseDayRental = 120m, BaseKmPrice = 1m };
    public CarTypePricing Truck { get; set; } = new() { BaseDayRental = 150m, BaseKmPrice = 1.5m };
}

public class CarTypePricing
{
    public decimal BaseDayRental { get; set; }
    public decimal BaseKmPrice { get; set; }
}
