namespace Capstone.Application.AdminDomain.Queries.GetStudentsAdmin
{
    public class GetStudentsAdminHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentsAdminQuery, GetStudentsAdminResult>
    {
        public async Task<GetStudentsAdminResult> Handle(GetStudentsAdminQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Teachers
                                        .AsNoTracking()
                                        .CountAsync(cancellationToken);

            var students = await dbContext.Students
                .Select(t => new GetStudentsAdminDto(
                    t.Id.Value,
                    t.StudentId.Value,
                    t.UserName.Value,
                    t.Email != null ? t.Email.Value : null,
                    t.Phone != null ? t.Phone.Value : null,
                    t.Avatar != null ? t.Avatar.Url : null,
                    t.IsActive.Value
                ))
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetStudentsAdminResult(new PaginationResult<GetStudentsAdminDto>
            (
                pageIndex,
                pageSize,
                totalCount,
                students
            ));
        }
    }
}
