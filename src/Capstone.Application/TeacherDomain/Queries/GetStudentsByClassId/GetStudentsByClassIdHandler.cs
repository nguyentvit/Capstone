using Capstone.Domain.ClassDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Queries.GetStudentsByClassId
{
    public class GetStudentsByClassIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentsByClassIdQuery, GetStudentsByClassIdResult>
    {
        public async Task<GetStudentsByClassIdResult> Handle(GetStudentsByClassIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var classId = ClassId.Of(query.ClassId);

            var classExist = await dbContext.Classes
                                        .AsNoTracking()
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
                                        .Where(x => x.Class.Id == classId && x.Subject != null && x.Subject.OwnerId == userId)
                                        .Select(x => new { x.Class.Students })
                                        .FirstOrDefaultAsync(cancellationToken);

            if (classExist == null)
                throw new ClassNotFoundException(classId.Value);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = classExist.Students.Count;

            var studentIds = classExist.Students.Select(x => x.StudentId).ToList();

            var students = await dbContext.Students
                                          .AsNoTracking()
                                          .Where(s => studentIds.Contains(s.Id))
                                          .Select(s => new GetStudentsByClassIdDto(s.Id.Value, s.StudentId.Value, s.UserName.Value))
                                          .Skip((pageIndex - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToListAsync(cancellationToken);

            return new GetStudentsByClassIdResult(new PaginationResult<GetStudentsByClassIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                students
                ));
        }
    }
}
