using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.AdminDomain.Queries.GetSystemSubjectById
{
    public class GetSystemSubjectByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetSystemSubjectByIdQuery, GetSystemSubjectByIdResult>
    {
        public async Task<GetSystemSubjectByIdResult> Handle(GetSystemSubjectByIdQuery query, CancellationToken cancellationToken)
        {
            var subjectId = SubjectId.Of(query.Id);

            var systemSubject = await dbContext.SystemSubjects
                                                .AsNoTracking()
                                                .Where(s => s.Id == subjectId)
                                                .Select(s => new GetSystemSubjectByIdResult(s.Id.Value, s.SubjectName.Value))
                                                .FirstOrDefaultAsync(cancellationToken);

            if (systemSubject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            return systemSubject;
        }
    }
}
