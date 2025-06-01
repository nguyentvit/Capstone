using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Queries.GetTeacherSubjectById
{
    public class GetTeacherSubjectByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetTeacherSubjectByIdQuery, GetTeacherSubjectByIdResult>
    {
        public async Task<GetTeacherSubjectByIdResult> Handle(GetTeacherSubjectByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var subjectId = SubjectId.Of(query.SubjectId);

            var teacherSubject = await dbContext.TeacherSubjects
                                                .AsNoTracking()
                                                .Where(s => s.Id == subjectId && s.OwnerId == userId)
                                                .Select(s => new GetTeacherSubjectByIdResult(s.Id.Value, s.SubjectName.Value))
                                                .FirstOrDefaultAsync(cancellationToken);

            if (teacherSubject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            return teacherSubject;
        }
    }
}
