namespace CarRental.Domain.Exceptions;

public class RentalNotFoundException : DomainException
{
    public RentalNotFoundException(string bookingNumber)
        : base($"Rental with booking number '{bookingNumber}' was not found.") { }
}
