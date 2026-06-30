using System.Text.RegularExpressions;
using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Services;

namespace CarRental.Domain;

public class RentalDomain
{
    public Guid Id { get; private set; }
    public string BookingNumber { get; private set; }
    public Guid VehicleId { get; private set; }
    public string RegistrationNumber { get; private set; }
    public CarType CarType { get; private set; }
    public string CustomerSocialSecurityNumber { get; private set; }
    public DateTimeOffset PickupDateTime { get; private set; }
    public int PickupMeterReading { get; private set; }
    public decimal BaseDayRental { get; private set; }
    public decimal BaseKmPrice { get; private set; }
    public DateTimeOffset? ReturnDateTime { get; private set; }
    public int? ReturnMeterReading { get; private set; }
    public decimal? TotalPrice { get; private set; }

    private const int MinimumRentalDays = 1;

    public bool IsReturned => ReturnDateTime.HasValue;

    public int? NumberOfDays => IsReturned
        ? Math.Max(MinimumRentalDays, (int)Math.Ceiling((ReturnDateTime!.Value - PickupDateTime).TotalDays))
        : null;

    public int? NumberOfKm => IsReturned
        ? ReturnMeterReading!.Value - PickupMeterReading
        : null;

    private RentalDomain(
        Guid id, string bookingNumber, Guid vehicleId, string registrationNumber,
        CarType carType, string customerSocialSecurityNumber,
        DateTimeOffset pickupDateTime, int pickupMeterReading,
        decimal baseDayRental, decimal baseKmPrice,
        DateTimeOffset? returnDateTime, int? returnMeterReading, decimal? totalPrice)
    {
        Id = id;
        BookingNumber = bookingNumber;
        VehicleId = vehicleId;
        RegistrationNumber = registrationNumber;
        CarType = carType;
        CustomerSocialSecurityNumber = customerSocialSecurityNumber;
        PickupDateTime = pickupDateTime;
        PickupMeterReading = pickupMeterReading;
        BaseDayRental = baseDayRental;
        BaseKmPrice = baseKmPrice;
        ReturnDateTime = returnDateTime;
        ReturnMeterReading = returnMeterReading;
        TotalPrice = totalPrice;
    }

    public static RentalDomain Load(
        Guid id, string bookingNumber, Guid vehicleId, string registrationNumber,
        CarType carType, string customerSocialSecurityNumber,
        DateTimeOffset pickupDateTime, int pickupMeterReading,
        decimal baseDayRental, decimal baseKmPrice,
        DateTimeOffset? returnDateTime, int? returnMeterReading, decimal? totalPrice) =>
        new(id, bookingNumber, vehicleId, registrationNumber, carType, customerSocialSecurityNumber,
            pickupDateTime, pickupMeterReading, baseDayRental, baseKmPrice,
            returnDateTime, returnMeterReading, totalPrice);

    public static RentalDomain RegisterPickup(
        VehicleDomain vehicle,
        string bookingNumber,
        string customerSocialSecurityNumber,
        DateTimeOffset pickupDateTime,
        int pickupMeterReading,
        decimal baseDayRental,
        decimal baseKmPrice)
    {
        if (string.IsNullOrWhiteSpace(bookingNumber))
            throw new DomainException("Booking number is required.");
        if (string.IsNullOrWhiteSpace(customerSocialSecurityNumber))
            throw new DomainException("Customer social security number is required.");
        if (!IsValidSsn(customerSocialSecurityNumber))
            throw new DomainException("Customer social security number must be in the format YYMMDD-XXXX or YYYYMMDD-XXXX.");
        if (pickupMeterReading < 0)
            throw new DomainException("Pickup meter reading cannot be negative.");

        return new RentalDomain(
            Guid.NewGuid(), bookingNumber, vehicle.Id, vehicle.RegistrationNumber,
            vehicle.CarType, customerSocialSecurityNumber,
            pickupDateTime, pickupMeterReading, baseDayRental, baseKmPrice,
            null, null, null);
    }

    public void RegisterReturn(DateTimeOffset returnDateTime, int returnMeterReading)
    {
        if (IsReturned)
            throw new DomainException("Car has already been returned for this booking.");
        if (returnDateTime <= PickupDateTime)
            throw new DomainException("Return date and time must be after the pickup date and time.");
        if (returnMeterReading < PickupMeterReading)
            throw new DomainException("Return meter reading cannot be less than pickup meter reading.");

        ReturnDateTime = returnDateTime;
        ReturnMeterReading = returnMeterReading;

        TotalPrice = CalculatePrice(NumberOfDays!.Value, NumberOfKm!.Value);
    }

    private decimal CalculatePrice(int numberOfDays, int numberOfKm) =>
        RentalPriceCalculator.Calculate(CarType, numberOfDays, numberOfKm, BaseDayRental, BaseKmPrice);

    private static bool IsValidSsn(string ssn) =>
        Regex.IsMatch(ssn, @"^(\d{8}|\d{6})-\d{4}$");
}
