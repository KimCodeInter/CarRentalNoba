using CarRental.Application.Mapping;
using CarRental.Contracts.Responses;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;
using CarRental.Domain.Services;
using ContractCarType = CarRental.Contracts.Enums.CarType;
using DomainCarType = CarRental.Domain.Enums.CarType;

namespace CarRental.Application.Handlers;

public class GetPriceEstimateHandler
{
    private readonly IPricingRepository _pricingRepository;

    public GetPriceEstimateHandler(IPricingRepository pricingRepository)
    {
        _pricingRepository = pricingRepository;
    }

    public async Task<PriceEstimateResponse> HandleAsync(ContractCarType carType, int numberOfDays, int numberOfKm, CancellationToken cancellationToken = default)
    {
        if (numberOfDays <= 0)
            throw new DomainException("Number of days must be greater than zero.");
        if (numberOfKm < 0)
            throw new DomainException("Number of km cannot be negative.");

        var domainCarType = carType.ToDomain();
        var currentRate = await _pricingRepository.GetCurrentAsync(domainCarType, cancellationToken)
            ?? throw new DomainException($"No pricing configured for '{carType}'");

        var estimatedPrice = RentalPriceCalculator.Calculate(domainCarType, numberOfDays, numberOfKm, currentRate.BaseDayRental, currentRate.BaseKmPrice);

        return new PriceEstimateResponse(carType, currentRate.BaseDayRental, currentRate.BaseKmPrice, numberOfDays, numberOfKm, estimatedPrice);
    }
}
