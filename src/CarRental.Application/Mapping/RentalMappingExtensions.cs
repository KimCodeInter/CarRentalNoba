using CarRental.Contracts.Responses;
using CarRental.Domain;

namespace CarRental.Application.Mapping;

public static class RentalMappingExtensions
{
    public static RegisterPickupResponse ToPickupResponse(this RentalDomain rental) =>
        new(rental.Id,
            rental.BookingNumber,
            rental.RegistrationNumber,
            rental.CarType.ToContract(),
            rental.PickupDateTime,
            rental.PickupMeterReading);

    public static RegisterReturnResponse ToReturnResponse(this RentalDomain rental) =>
        new(rental.Id,
            rental.BookingNumber,
            rental.PickupDateTime,
            rental.ReturnDateTime!.Value,
            rental.PickupMeterReading,
            rental.ReturnMeterReading!.Value,
            rental.NumberOfDays!.Value,
            rental.NumberOfKm!.Value,
            rental.TotalPrice!.Value);

    public static RentalDetailsResponse ToDetailsResponse(this RentalDomain rental) =>
        new(rental.Id,
            rental.BookingNumber,
            rental.RegistrationNumber,
            rental.CustomerSocialSecurityNumber,
            rental.CarType.ToContract(),
            rental.PickupDateTime,
            rental.PickupMeterReading,
            rental.ReturnDateTime,
            rental.ReturnMeterReading,
            rental.TotalPrice,
            rental.IsReturned);
}
