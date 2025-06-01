namespace Capstone.Application.AdminDomain.Queries.GetSystemSubjects
{
    public class GetSystemSubjectsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetSystemSubjectsQuery, GetSystemSubjectsResult>
    {
        public async Task<GetSystemSubjectsResult> Handle(GetSystemSubjectsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.SystemSubjects.AsNoTracking().CountAsync(cancellationToken);
            var systemSubjects = await dbContext.SystemSubjects
                                                .AsNoTracking()
                                                .Select(s => new GetSystemSubjectsDto(
                                                    s.Id.Value,
                                                    s.SubjectName.Value
                                                    ))
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync(cancellationToken);

            return new GetSystemSubjectsResult(new PaginationResult<GetSystemSubjectsDto>(
                pageIndex,
                pageSize,
                totalCount,
                systemSubjects
                ));
                                                
        }
    }
}
