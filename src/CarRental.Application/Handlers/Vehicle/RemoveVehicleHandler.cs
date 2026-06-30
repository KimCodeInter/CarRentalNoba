using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class RemoveVehicleHandler
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRentalRepository _rentalRepository;

    public RemoveVehicleHandler(IVehicleRepository vehicleRepository, IRentalRepository rentalRepository)
    {
        _vehicleRepository = vehicleRepository;
        _rentalRepository = rentalRepository;
    }

    public async Task HandleAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var vehicle = await _vehicleRepository.GetByRegistrationNumberAsync(registrationNumber, cancellationToken)
            ?? throw new VehicleNotFoundException(registrationNumber);

        var hasActiveRental = await _rentalRepository.HasActiveRentalAsync(vehicle.Id, cancellationToken);
        if (hasActiveRental)
            throw new DomainException($"Vehicle '{registrationNumber}' cannot be removed while it has an active rental.");

        await _vehicleRepository.RemoveAsync(vehicle, cancellationToken);
    }
}
