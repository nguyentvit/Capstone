using Capstone.Application.Admin.Commands.CreateRescue;
using Capstone.Application.Interface;
using Capstone.Domain.UserAccess.ValueObjects;
using Capstone.Domain.RescueTeam.Models;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.RescueTeam.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Moq;
using Xunit;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Capstone.Application.Common.Exceptions;
namespace Capstone.Application.Tests.Admin.Commands.CreateRescue;

public class CreateRescueHandlerTests
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly CreateRescueHandler _handler;

    public CreateRescueHandlerTests()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _handler = new CreateRescueHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Rescue_And_Return_Result()
    {
        // Arrange
        var command = new CreateRescueCommand(
            "Rescue Team 1", "0123456789", "District A", "Ward B", "Province C",
            105.85, 21.02, Guid.NewGuid()
        );

        var managerId = UserId.Of(command.ManagerId);
        var mockUser = User.Of(managerId, UserName.Of("TestUser"), Email.Of("test@example.com"));

        _dbContextMock.Setup(db => db.AppUsers.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                      .Returns(ValueTask.FromResult<User?>(mockUser));

        _dbContextMock.Setup(db => db.Rescues.Add(It.IsAny<Rescue>()));

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();

        _dbContextMock.Verify(db => db.Rescues.Add(It.IsAny<Rescue>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Not_Found()
    {
        // Arrange
        var command = new CreateRescueCommand(
            "Rescue Team 1", "0123456789", "District A", "Ward B", "Province C",
            105.85, 21.02, Guid.NewGuid()
        );

        _dbContextMock.Setup(db => db.AppUsers.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                      .Returns(ValueTask.FromResult<User?>(null));

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();

        _dbContextMock.Verify(db => db.Rescues.Add(It.IsAny<Rescue>()), Times.Never);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
