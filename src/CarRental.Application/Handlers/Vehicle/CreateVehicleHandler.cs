using CarRental.Application.Mapping;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using CarRental.Domain;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class CreateVehicleHandler
{
    private readonly IVehicleRepository _repository;

    public CreateVehicleHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<VehicleResponse> HandleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByRegistrationNumberAsync(request.RegistrationNumber, cancellationToken);
        if (existing is not null)
            throw new DomainException($"A vehicle with registration number '{request.RegistrationNumber}' already exists.");

        var vehicle = VehicleDomain.Create(request.RegistrationNumber, request.CarType.ToDomain());

        await _repository.AddAsync(vehicle, cancellationToken);

        return vehicle.ToVehicleResponse();
    }
}
