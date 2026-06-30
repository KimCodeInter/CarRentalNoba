using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetVehicleRentalHistoryHandler
{
    private readonly IRentalRepository _repository;

    public GetVehicleRentalHistoryHandler(IRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RentalDetailsResponse>> HandleAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var rentals = await _repository.GetByVehicleRegistrationAsync(registrationNumber, cancellationToken);
        return rentals.Select(r => r.ToDetailsResponse()).ToList();
    }
}
