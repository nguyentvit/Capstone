namespace Capstone.Application.AdminDomain.Queries.GetTeachers
{
    public class GetTeachersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetTeachersQuery, GetTeachersResult>
    {
        public async Task<GetTeachersResult> Handle(GetTeachersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Teachers
                                        .AsNoTracking()
                                        .CountAsync(cancellationToken);

            var teachers = await dbContext.Teachers
                .Select(t => new GetTeachersResultDto(
                    t.Id.Value,
                    t.TeacherId.Value,
                    t.UserName.Value,
                    t.Email != null ? t.Email.Value : null,
                    t.Phone != null ? t.Phone.Value : null,
                    t.Avatar != null ? t.Avatar.Url : null,
                    t.IsActive.Value
                ))
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetTeachersResult(new PaginationResult<GetTeachersResultDto>
            (
                pageIndex,
                pageSize,
                totalCount,
                teachers
            ));
        }
    }
}
