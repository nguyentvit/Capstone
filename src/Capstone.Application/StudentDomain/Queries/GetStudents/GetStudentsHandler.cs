namespace Capstone.Application.StudentDomain.Queries.GetStudents
{
    public class GetStudentsHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentsQuery, GetStudentsResult>
    {
        public async Task<GetStudentsResult> Handle(GetStudentsQuery query, CancellationToken cancellationToken)
        {
            var studentsQuery = dbContext.Students.AsNoTracking()
                                                  .Where(s => ((string)(object)s.StudentId).StartsWith(query.KeySearchStudentId))
                                                  .AsQueryable();

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await studentsQuery.CountAsync(cancellationToken);

            var students = await studentsQuery
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(s => new GetStudentsDto(s.Id.Value, s.StudentId.Value, s.UserName.Value))
                                        .ToListAsync(cancellationToken);

            return new GetStudentsResult(new PaginationResult<GetStudentsDto>(
                pageIndex,
                pageSize,
                totalCount,
                students
                ));

        }
    }
}
