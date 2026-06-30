using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetActiveRentalsHandler
{
    private readonly IRentalRepository _repository;

    public GetActiveRentalsHandler(IRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RentalDetailsResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var rentals = await _repository.GetAllActiveAsync(cancellationToken);
        return rentals.Select(r => r.ToDetailsResponse()).ToList();
    }
}
