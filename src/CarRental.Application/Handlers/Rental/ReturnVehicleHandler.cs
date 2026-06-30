using CarRental.Application.Mapping;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Handlers;

public class ReturnVehicleHandler
{
    private readonly IRentalRepository _repository;

    public ReturnVehicleHandler(IRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegisterReturnResponse> HandleAsync(RegisterReturnRequest request, CancellationToken cancellationToken = default)
    {
        var rental = await _repository.GetByBookingNumberAsync(request.BookingNumber, cancellationToken)
            ?? throw new RentalNotFoundException(request.BookingNumber);

        rental.RegisterReturn(request.ReturnDateTime, request.ReturnMeterReading);

        await _repository.UpdateAsync(rental, cancellationToken);

        return rental.ToReturnResponse();
    }
}
