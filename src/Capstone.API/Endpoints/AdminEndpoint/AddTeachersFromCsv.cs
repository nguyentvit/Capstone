using Capstone.Application.AdminDomain.Commands.ImportCsvAddTeachers;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public record AddTeachersFromCsvRequest(IFormFile Csv);
    public class AddTeachersFromCsv : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/teachers/csv", async (ISender sender, [FromForm] AddTeachersFromCsvRequest request) =>
            {
                var command = new ImportCsvAddTeachersCommand(request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            })
                .RequireAuthorization(PolicyConstant.ADMIN)
                .DisableAntiforgery();
        }
    }
}
