namespace Capstone.Application.AdminDomain.Queries.GetStudents
{
    public class GetStudentsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentsQuery, GetStudentsResult>
    {
        public async Task<GetStudentsResult> Handle(GetStudentsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Students.AsNoTracking().CountAsync(cancellationToken);

            var students = await dbContext.Students
                                        .AsNoTracking()
                                        .Select(s => new GetStudentsResultDto(
                                            s.Id.Value,
                                            s.StudentId.Value,
                                            s.UserName.Value,
                                            (s.Email != null) ? s.Email.Value : null,
                                            (s.Phone != null) ? s.Phone.Value : null,
                                            (s.Avatar != null) ? s.Avatar.Url : null,
                                            s.IsActive.Value
                                            ))
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

            return new GetStudentsResult(new PaginationResult<GetStudentsResultDto>
            (
                pageIndex,
                pageSize,
                totalCount,
                students
            ));
        }
    }
}
