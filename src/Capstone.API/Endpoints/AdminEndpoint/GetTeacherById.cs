using Capstone.Application.AdminDomain.Queries.GetTeacherById;

namespace Capstone.API.Endpoints.AdminEndpoint
{
    public class GetTeacherById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/admin/teachers/{id}", async (ISender sender, Guid id) =>
            {
                var query = new GetTeacherByIdQuery(id);

                var response = await sender.Send(query);

                return Results.Ok(response);
            }).RequireAuthorization(PolicyConstant.ADMIN);
        }
    }
}
