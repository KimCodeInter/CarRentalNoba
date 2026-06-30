namespace CarRental.Domain.Exceptions;

public class VehicleNotFoundException : DomainException
{
    public VehicleNotFoundException(string registrationNumber)
        : base($"Vehicle with registration number '{registrationNumber}' was not found.") { }
}
