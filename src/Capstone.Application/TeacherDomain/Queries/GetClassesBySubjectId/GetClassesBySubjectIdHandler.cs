using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Queries.GetClassesBySubjectId
{
    public class GetClassesBySubjectIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetClassesBySubjectIdQuery, GetClassesBySubjectIdResult>
    {
        public async Task<GetClassesBySubjectIdResult> Handle(GetClassesBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.UserId);
            var subjectId = SubjectId.Of(query.SubjectId);

            var teacherSubject = await dbContext.TeacherSubjects
                                                .AsNoTracking()
                                                .Where(s => s.Id == subjectId && s.OwnerId == userId)
                                                .Select(s => new { s.ClassIds, s.Id })
                                                .FirstOrDefaultAsync(cancellationToken);

            if (teacherSubject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = teacherSubject.ClassIds.Count;

            var classes = await dbContext.Classes
                                        .AsNoTracking()
                                        .Where(c => teacherSubject.ClassIds.Contains(c.Id))
                                        .Select(c => new GetClassesBySubjectIdDto(c.Id.Value, c.Name.Value))
                                        .Skip((pageIndex - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

            return new GetClassesBySubjectIdResult(new PaginationResult<GetClassesBySubjectIdDto>(
                pageIndex,
                pageSize,
                totalCount,
                classes
                ));
        }
    }
}
