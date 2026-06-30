using CarRental.Application.Handlers;
using CarRental.Contracts.Enums;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    [HttpPost("pickup")]
    [ProducesResponseType(typeof(RegisterPickupResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterPickup(
        [FromBody] RegisterPickupRequest request,
        [FromServices] PickupVehicleHandler pickupVehicle,
        CancellationToken cancellationToken)
    {
        var response = await pickupVehicle.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetRentedVehicle), new { bookingNumber = response.BookingNumber }, response);
    }

    [HttpPost("return")]
    [ProducesResponseType(typeof(RegisterReturnResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterReturn(
        [FromBody] RegisterReturnRequest request,
        [FromServices] ReturnVehicleHandler returnVehicle,
        CancellationToken cancellationToken)
    {
        var response = await returnVehicle.HandleAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("estimate")]
    [ProducesResponseType(typeof(PriceEstimateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPriceEstimate(
        [FromQuery] CarType carType,
        [FromQuery] int numberOfDays,
        [FromQuery] int numberOfKm,
        [FromServices] GetPriceEstimateHandler getPriceEstimate,
        CancellationToken cancellationToken)
    {
        var response = await getPriceEstimate.HandleAsync(carType, numberOfDays, numberOfKm, cancellationToken);
        return Ok(response);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<RentalDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveRentals(
        [FromServices] GetActiveRentalsHandler getActiveRentals,
        CancellationToken cancellationToken)
    {
        var response = await getActiveRentals.HandleAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("customer/{ssn}")]
    [ProducesResponseType(typeof(IReadOnlyList<RentalDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(
        string ssn,
        [FromServices] GetRentalsByCustomerHandler getRentalsByCustomer,
        CancellationToken cancellationToken)
    {
        var response = await getRentalsByCustomer.HandleAsync(ssn, cancellationToken);
        return Ok(response);
    }

    [HttpGet("vehicle/{registrationNumber}")]
    [ProducesResponseType(typeof(IReadOnlyList<RentalDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicleRentalHistory(
        string registrationNumber,
        [FromServices] GetVehicleRentalHistoryHandler getVehicleRentalHistory,
        CancellationToken cancellationToken)
    {
        var response = await getVehicleRentalHistory.HandleAsync(registrationNumber, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{bookingNumber}")]
    [ProducesResponseType(typeof(RentalDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRentedVehicle(
        string bookingNumber,
        [FromServices] GetRentedVehicleHandler getRentedVehicle,
        CancellationToken cancellationToken)
    {
        var response = await getRentedVehicle.HandleAsync(bookingNumber, cancellationToken);
        if (response is null)
            return NotFound(new { error = $"Rental with booking number '{bookingNumber}' was not found." });

        return Ok(response);
    }
}
