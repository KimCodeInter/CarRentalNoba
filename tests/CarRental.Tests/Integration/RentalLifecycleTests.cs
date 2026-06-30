using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CarRental.Contracts.Responses;
using FluentAssertions;

namespace CarRental.Tests.Integration;

public class RentalLifecycleTests : IClassFixture<CarRentalApiFactory>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly DateTimeOffset PickupAt = new(2026, 1, 10, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ReturnAt = new(2026, 1, 13, 10, 0, 0, TimeSpan.Zero);

    public RentalLifecycleTests(CarRentalApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RentSmallCarLifecycle()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/vehicles", new
        {
            registrationNumber = "ABC-001",
            carType = 1
        });
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var vehicle = await createResponse.Content.ReadFromJsonAsync<VehicleResponse>(JsonOptions);

        var availableResponse = await _client.GetFromJsonAsync<List<VehicleResponse>>("/api/vehicles/available", JsonOptions);
        availableResponse.Should().Contain(v => v.RegistrationNumber == vehicle!.RegistrationNumber);

        var pickupResponse = await _client.PostAsJsonAsync("/api/rentals/pickup", new
        {
            registrationNumber = vehicle!.RegistrationNumber,
            customerSocialSecurityNumber = "19930101-1234",
            pickupDateTime = PickupAt,
            pickupMeterReading = 50000
        });
        pickupResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var pickup = await pickupResponse.Content.ReadFromJsonAsync<RegisterPickupResponse>(JsonOptions);
        pickup!.RegistrationNumber.Should().Be(vehicle.RegistrationNumber);
        pickup.BookingNumber.Should().StartWith("BK-");

        var availableAfter = await _client.GetFromJsonAsync<List<VehicleResponse>>("/api/vehicles/available", JsonOptions);
        availableAfter.Should().NotContain(v => v.RegistrationNumber == vehicle.RegistrationNumber);

        var activeResponse = await _client.GetFromJsonAsync<List<RentalDetailsResponse>>("/api/rentals/active", JsonOptions);
        activeResponse.Should().Contain(r => r.BookingNumber == pickup.BookingNumber && !r.IsReturned);

        var detailResponse = await _client.GetAsync($"/api/rentals/{pickup.BookingNumber}");
        detailResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var detail = await detailResponse.Content.ReadFromJsonAsync<RentalDetailsResponse>(JsonOptions);
        detail!.BookingNumber.Should().Be(pickup.BookingNumber);
        detail.PickupMeterReading.Should().Be(50000);
        detail.IsReturned.Should().BeFalse();

        var returnResponse = await _client.PostAsJsonAsync("/api/rentals/return", new
        {
            bookingNumber = pickup.BookingNumber,
            returnDateTime = ReturnAt,
            returnMeterReading = 50350
        });
        returnResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var returned = await returnResponse.Content.ReadFromJsonAsync<RegisterReturnResponse>(JsonOptions);
        returned!.NumberOfDays.Should().Be(3);
        returned.NumberOfKm.Should().Be(350);
        returned.TotalPrice.Should().Be(300m);

        var historyResponse = await _client.GetFromJsonAsync<List<RentalDetailsResponse>>(
            $"/api/rentals/vehicle/{vehicle.RegistrationNumber}", JsonOptions);
        historyResponse.Should().ContainSingle(r => r.BookingNumber == pickup.BookingNumber && r.IsReturned);
    }

    [Fact]
    public async Task RentCombiCarWithKilometerCharges()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/vehicles", new { registrationNumber = "ABC-002", carType = 2 });
        var vehicle = await createResponse.Content.ReadFromJsonAsync<VehicleResponse>(JsonOptions);

        var pickupResponse = await _client.PostAsJsonAsync("/api/rentals/pickup", new
        {
            registrationNumber = vehicle!.RegistrationNumber,
            customerSocialSecurityNumber = "19930101-5678",
            pickupDateTime = PickupAt,
            pickupMeterReading = 10000
        });
        var pickup = await pickupResponse.Content.ReadFromJsonAsync<RegisterPickupResponse>(JsonOptions);

        var returnResponse = await _client.PostAsJsonAsync("/api/rentals/return", new
        {
            bookingNumber = pickup!.BookingNumber,
            returnDateTime = ReturnAt,
            returnMeterReading = 10100
        });
        var returned = await returnResponse.Content.ReadFromJsonAsync<RegisterReturnResponse>(JsonOptions);

        returned!.TotalPrice.Should().Be(568m);
    }

    [Fact]
    public async Task ViewRentalHistoryByCustomer()
    {
        var ssn = "19930101-9999";

        var vehicle1 = await (await _client.PostAsJsonAsync("/api/vehicles", new { registrationNumber = "ABC-003", carType = 1 }))
            .Content.ReadFromJsonAsync<VehicleResponse>(JsonOptions);
        var vehicle2 = await (await _client.PostAsJsonAsync("/api/vehicles", new { registrationNumber = "ABC-004", carType = 1 }))
            .Content.ReadFromJsonAsync<VehicleResponse>(JsonOptions);

        var pickup1 = await (await _client.PostAsJsonAsync("/api/rentals/pickup", new
        {
            registrationNumber = vehicle1!.RegistrationNumber,
            customerSocialSecurityNumber = ssn,
            pickupDateTime = PickupAt,
            pickupMeterReading = 0
        })).Content.ReadFromJsonAsync<RegisterPickupResponse>(JsonOptions);

        await _client.PostAsJsonAsync("/api/rentals/return", new
        {
            bookingNumber = pickup1!.BookingNumber,
            returnDateTime = ReturnAt,
            returnMeterReading = 300
        });

        await (await _client.PostAsJsonAsync("/api/rentals/pickup", new
        {
            registrationNumber = vehicle2!.RegistrationNumber,
            customerSocialSecurityNumber = ssn,
            pickupDateTime = ReturnAt.AddDays(1),
            pickupMeterReading = 0
        })).Content.ReadFromJsonAsync<RegisterPickupResponse>(JsonOptions);

        var customerRentals = await _client.GetFromJsonAsync<List<RentalDetailsResponse>>(
            $"/api/rentals/customer/{ssn}", JsonOptions);

        customerRentals.Should().HaveCount(2);
        customerRentals.Should().AllSatisfy(r => r.CustomerSocialSecurityNumber.Should().Be(ssn));
    }
}
