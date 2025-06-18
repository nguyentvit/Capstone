using Capstone.Domain.ClassDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Queries.GetStudentsAvailableToAddIntoClass
{
    public class GetStudentsAvailableToAddIntoClassHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentsAvailableToAddIntoClassQuery, GetStudentsAvailableToAddIntoClassResult>
    {
        public async Task<GetStudentsAvailableToAddIntoClassResult> Handle(GetStudentsAvailableToAddIntoClassQuery query, CancellationToken cancellationToken)
        {
            var classExist = await dbContext.Classes
                                         .AsNoTracking()
                                         .Where(c => c.Id == ClassId.Of(query.ClassId))
                                         .FirstOrDefaultAsync(cancellationToken);

            if (classExist == null)
                throw new ClassNotFoundException(query.ClassId);

            var studentIds = classExist.Students.Select(s => s.StudentId).ToList();

            var studentsQuery = dbContext.Students.AsNoTracking()
                                                  .Where(s => ((string)(object)s.StudentId).StartsWith(query.KeySearchStudentId) && !studentIds.Contains(s.Id))
                                                  .AsQueryable();

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await studentsQuery.CountAsync(cancellationToken);

            var students = await studentsQuery
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(s => new GetStudentsAvailableToAddIntoClassDto(s.Id.Value, s.StudentId.Value, s.UserName.Value))
                                        .ToListAsync(cancellationToken);

            return new GetStudentsAvailableToAddIntoClassResult(new PaginationResult<GetStudentsAvailableToAddIntoClassDto>(
                pageIndex,
                pageSize,
                totalCount,
                students
                ));
        }
    }
}
