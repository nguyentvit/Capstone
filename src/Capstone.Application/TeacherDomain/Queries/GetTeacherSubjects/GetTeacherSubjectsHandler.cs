namespace Capstone.Application.TeacherDomain.Queries.GetTeacherSubjects
{
    public class GetTeacherSubjectsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetTeacherSubjectsQuery, GetTeacherSubjectsResult>
    {
        public async Task<GetTeacherSubjectsResult> Handle(GetTeacherSubjectsQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var userId = UserId.Of(query.UserId);

            var totalCount = await dbContext.TeacherSubjects
                                            .AsNoTracking()
                                            .Where(s => s.OwnerId == userId)
                                            .CountAsync(cancellationToken);

            var teacherSubjects = await dbContext.TeacherSubjects
                                                .AsNoTracking()
                                                .Where(s => s.OwnerId == userId)
                                                .Select(s => new GetTeacherSubjectsDto(s.Id.Value, s.SubjectName.Value))
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync(cancellationToken);

            return new GetTeacherSubjectsResult(new PaginationResult<GetTeacherSubjectsDto>(
                pageIndex,
                pageSize,
                totalCount,
                teacherSubjects
                ));
        }
    }
}
