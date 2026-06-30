using CarRental.Application.Handlers;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    [HttpGet("available")]
    [ProducesResponseType(typeof(IReadOnlyList<VehicleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable(
        [FromServices] GetAvailableVehiclesHandler getAvailableVehicles,
        CancellationToken cancellationToken)
    {
        var response = await getAvailableVehicles.HandleAsync(cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{registrationNumber}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(
        string registrationNumber,
        [FromServices] RemoveVehicleHandler removeVehicle,
        CancellationToken cancellationToken)
    {
        await removeVehicle.HandleAsync(registrationNumber, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateVehicleRequest request,
        [FromServices] CreateVehicleHandler createVehicle,
        CancellationToken cancellationToken)
    {
        var response = await createVehicle.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
}
