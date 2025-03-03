namespace Capstone.Application.Admin.Commands.CreateRescue;
public record CreateRescueCommand(string RescueName, string Phone, string District, string Ward, string Province, double Longitude, double Latitude, Guid ManagerId) : ICommand<CreateRescueResult>;
public record CreateRescueResult(Guid Id);
public class CreateRescueCommandValidator : AbstractValidator<CreateRescueCommand>
{
    public CreateRescueCommandValidator()
    {
        RuleFor(x => x.RescueName)
            .NotEmpty()
            .WithMessage("Rescue name is required")
            .MaximumLength(100)
            .WithMessage("Rescue name must not exceed 100 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{9,11}$").WithMessage("Phone number must be between 9 and 11 digits.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees.");

        RuleFor(x => x.ManagerId)
            .NotEmpty().WithMessage("ManagerId is required.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required.");

        RuleFor(x => x.Ward)
            .NotEmpty().WithMessage("Ward is required.");

        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("Province is required.");
    }
}