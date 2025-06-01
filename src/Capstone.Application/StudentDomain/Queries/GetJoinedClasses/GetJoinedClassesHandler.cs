namespace Capstone.Application.StudentDomain.Queries.GetJoinedClasses
{
    public class GetJoinedClassesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetJoinedClassesQuery, GetJoinedClassesResult>
    {
        public async Task<GetJoinedClassesResult> Handle(GetJoinedClassesQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var joinedClassesQuery = dbContext.Classes
                                              .AsNoTracking()
                                              .Where(c => c.Students.Any(s => s.StudentId == userId));

            var totalCount = await joinedClassesQuery.CountAsync(cancellationToken);
            var joineddClasses = await joinedClassesQuery
                                                .Select(c => new GetJoinedClassesDto(c.Id.Value, c.Name.Value))
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync(cancellationToken);

            return new GetJoinedClassesResult(new PaginationResult<GetJoinedClassesDto>(
                pageIndex,
                pageSize,
                totalCount,
                joineddClasses
                ));
        }
    }
}
