using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetAvailableVehiclesHandler
{
    private readonly IVehicleRepository _repository;

    public GetAvailableVehiclesHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<VehicleResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _repository.GetAvailableAsync(cancellationToken);
        return vehicles.Select(v => v.ToVehicleResponse()).ToList();
    }
}
