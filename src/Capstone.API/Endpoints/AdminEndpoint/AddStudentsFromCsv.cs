using Capstone.Application.AdminDomain.Commands.ImportCsvAddStudents;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public record AddStudentsFromCsvRequest(IFormFile Csv);
    public class AddStudentsFromCsv : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/admin/students/csv", async (ISender sender, [FromForm] AddStudentsFromCsvRequest request) =>
            {
                var command = new ImportCsvAddStudentsCommand(request.Csv);

                var response = await sender.Send(command);

                return Results.Ok(response);
            })
                .RequireAuthorization(PolicyConstant.ADMIN)
                .DisableAntiforgery();
        }
    }
}
