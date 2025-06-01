using Capstone.Domain.ClassDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Queries.GetClassById
{
    public class GetClassByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetClassByIdQuery, GetClassByIdResult>
    {
        public async Task<GetClassByIdResult> Handle(GetClassByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var classId = ClassId.Of(query.ClassId);

            var classExist = await dbContext.Classes
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
                                        .Select(x => new GetClassByIdResult(x.Class.Name.Value))
                                        .FirstOrDefaultAsync(cancellationToken);

            if (classExist == null)
                throw new ClassNotFoundException(classId.Value);

            return classExist;
        }
    }
}
