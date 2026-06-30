using CarRental.Application.Handlers;
using CarRental.Contracts.Requests;
using CarRental.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(PricingRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetPricing(
        [FromBody] SetPricingRequest request,
        [FromServices] SetPricingHandler setPricing,
        CancellationToken cancellationToken)
    {
        var response = await setPricing.HandleAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PricingRateResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentPricing(
        [FromServices] GetCurrentPricingHandler getCurrentPricing,
        CancellationToken cancellationToken)
    {
        var response = await getCurrentPricing.HandleAsync(cancellationToken);
        return Ok(response);
    }
}
