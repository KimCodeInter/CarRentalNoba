using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class GetRentedVehicleHandler
{
    private readonly IRentalRepository _repository;

    public GetRentedVehicleHandler(IRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<RentalDetailsResponse?> HandleAsync(string bookingNumber, CancellationToken cancellationToken = default)
    {
        var rental = await _repository.GetByBookingNumberAsync(bookingNumber, cancellationToken);
        return rental?.ToDetailsResponse();
    }
}
