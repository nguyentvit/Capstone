using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Capstone.API.Endpoints.Admin;
using Capstone.Application.Admin.Commands.CreateRescue;
using FluentAssertions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
public class CreateRescueTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<ISender> _mockSender;

    public CreateRescueTests(WebApplicationFactory<Program> factory)
    {
        _mockSender = new Mock<ISender>();

        // Khởi tạo client với test server
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mockSender.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task CreateRescue_ShouldReturn200Ok_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateRescueRequest(
            "Rescue Center 1",
            "0123456789",
            "District A",
            "Ward B",
            "Province C",
            106.6789,
            10.7765,
            Guid.NewGuid()
        );

        var expectedResult = new CreateRescueResult(Guid.NewGuid());
        _mockSender.Setup(s => s.Send(It.IsAny<CreateRescueCommand>(), default))
                   .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PostAsJsonAsync("/admin/secures", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<CreateRescueResponse>();
        responseData.Should().NotBeNull();
        responseData!.Id.Should().Be(expectedResult.Id);
    }

    [Fact]
    public async Task CreateRescue_ShouldReturn401Unauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = new CreateRescueRequest(
            "Rescue Center 1",
            "0123456789",
            "District A",
            "Ward B",
            "Province C",
            106.6789,
            10.7765,
            Guid.NewGuid()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/admin/secures", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
