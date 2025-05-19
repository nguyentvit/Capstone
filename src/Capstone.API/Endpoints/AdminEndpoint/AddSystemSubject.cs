using Capstone.Application.AdminDomain.Commands.AddSystemSubject;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public record AddSystemSubjectRequest(string SubjectName);
    public class AddSystemSubject : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/subjects", async (ISender sender, AddSystemSubjectRequest request) =>
            {
                var command = new AddSystemSubjectCommand(request.SubjectName);

                var response = await sender.Send(command);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
