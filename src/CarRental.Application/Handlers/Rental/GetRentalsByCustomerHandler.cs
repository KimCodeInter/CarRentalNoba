using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetRentalsByCustomerHandler
{
    private readonly IRentalRepository _repository;

    public GetRentalsByCustomerHandler(IRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RentalDetailsResponse>> HandleAsync(string customerSsn, CancellationToken cancellationToken = default)
    {
        var rentals = await _repository.GetByCustomerSsnAsync(customerSsn, cancellationToken);
        return rentals.Select(r => r.ToDetailsResponse()).ToList();
    }
}
