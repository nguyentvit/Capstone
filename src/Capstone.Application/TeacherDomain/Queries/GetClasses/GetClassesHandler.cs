namespace Capstone.Application.TeacherDomain.Queries.GetClasses
{
    public class GetClassesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetClassesQuery, GetClassesResult>
    {
        public async Task<GetClassesResult> Handle(GetClassesQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var classQuery = dbContext.Classes
                                  .GroupJoin(
                                      dbContext.TeacherSubjects,
                                      c => c.SubjectId,
                                      s => s.Id,
                                      (classEntity, subjects) => new { classEntity, subjects })
                                  .SelectMany(
                                      x => x.subjects.DefaultIfEmpty(),
                                      (x, subject) => new
                                      {
                                          Class = x.classEntity,
                                          Subject = subject
                                      })
                                  .Where(x => x.Subject != null && x.Subject.OwnerId == userId);

            var totalCount = await classQuery.CountAsync(cancellationToken);

            var classes = await classQuery
                                .Select(x => new GetClassesDto(x.Class.Id.Value, x.Class.Name.Value))
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);

            return new GetClassesResult(new PaginationResult<GetClassesDto>(
                pageIndex,
                pageSize,
                totalCount,
                classes
                ));
        }
    }
}
