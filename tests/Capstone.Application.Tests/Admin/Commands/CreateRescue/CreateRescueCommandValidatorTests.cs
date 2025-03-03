using Capstone.Application.Admin.Commands.CreateRescue;
using FluentValidation.TestHelper;
using Xunit;

public class CreateRescueCommandValidatorTests
{
    private readonly CreateRescueCommandValidator _validator;

    public CreateRescueCommandValidatorTests()
    {
        _validator = new CreateRescueCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_RescueName_Is_Empty()
    {
        var command = new CreateRescueCommand("", "0123456789", "District", "Ward", "Province", 105.85, 21.02, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.RescueName);
    }

    [Fact]
    public void Should_Have_Error_When_Phone_Is_Invalid()
    {
        var command = new CreateRescueCommand("Rescue Team", "123", "District", "Ward", "Province", 105.85, 21.02, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Have_Error_When_Longitude_Is_Out_Of_Range()
    {
        var command = new CreateRescueCommand("Rescue Team", "0123456789", "District", "Ward", "Province", 200, 21.02, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Longitude);
    }

    [Fact]
    public void Should_Have_Error_When_Latitude_Is_Out_Of_Range()
    {
        var command = new CreateRescueCommand("Rescue Team", "0123456789", "District", "Ward", "Province", 105.85, 100, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Latitude);
    }

    [Fact]
    public void Should_Have_Error_When_ManagerId_Is_Empty()
    {
        var command = new CreateRescueCommand("Rescue Team", "0123456789", "District", "Ward", "Province", 105.85, 21.02, Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.ManagerId);
    }

    [Fact]
    public void Should_Have_Error_When_Location_Fields_Are_Empty()
    {
        var command = new CreateRescueCommand("Rescue Team", "0123456789", "", "", "", 105.85, 21.02, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.District);
        result.ShouldHaveValidationErrorFor(x => x.Ward);
        result.ShouldHaveValidationErrorFor(x => x.Province);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Command()
    {
        var command = new CreateRescueCommand("Rescue Team", "0123456789", "District", "Ward", "Province", 105.85, 21.02, Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
