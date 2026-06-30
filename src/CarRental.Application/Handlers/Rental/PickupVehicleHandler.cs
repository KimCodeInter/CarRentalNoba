using CarRental.Application.Mapping;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using CarRental.Domain;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class PickupVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IPricingRepository _pricingRepository;

    public PickupVehicleHandler(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IPricingRepository pricingRepository)
    {
        _vehicleRepository = vehicleRepository;
        _rentalRepository = rentalRepository;
        _pricingRepository = pricingRepository;
    }

    public async Task<RegisterPickupResponse> HandleAsync(RegisterPickupRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByRegistrationNumberAsync(request.RegistrationNumber, cancellationToken)
            ?? throw new VehicleNotFoundException(request.RegistrationNumber);

        var isAlreadyRented = await _rentalRepository.HasActiveRentalAsync(vehicle.Id, cancellationToken);
        if (isAlreadyRented)
            throw new DomainException($"Vehicle '{request.RegistrationNumber}' is already on an active rental.");

        var currentRate = await _pricingRepository.GetCurrentAsync(vehicle.CarType, cancellationToken)
            ?? throw new DomainException($"No pricing configured for '{vehicle.CarType}'. Set rates via POST /api/pricing before registering pickups.");

        var bookingNumber = GenerateBookingNumber(request.PickupDateTime);

        var rental = RentalDomain.RegisterPickup(
            vehicle,
            bookingNumber,
            request.CustomerSocialSecurityNumber,
            request.PickupDateTime,
            request.PickupMeterReading,
            currentRate.BaseDayRental,
            currentRate.BaseKmPrice);

        await _rentalRepository.AddAsync(rental, cancellationToken);

        return rental.ToPickupResponse();
    }

    private static string GenerateBookingNumber(DateTimeOffset pickupDateTime) =>
        $"BK-{pickupDateTime:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
}
