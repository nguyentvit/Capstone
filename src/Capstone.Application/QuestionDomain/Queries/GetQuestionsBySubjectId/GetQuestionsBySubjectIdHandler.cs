using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.QuestionDomain.Queries.GetQuestionsBySubjectId
{
    public class GetQuestionsBySubjectIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetQuestionsBySubjectIdQuery, GetQuestionsBySubjectIdResult>
    {
        public async Task<GetQuestionsBySubjectIdResult> Handle(GetQuestionsBySubjectIdQuery query, CancellationToken cancellationToken)
        {
            //var userId = UserId.Of(query.UserId);
            //var subjectId = SubjectId.Of(query.SubjectId);

            //var subject = await dbContext.Subjects
            //                             .AsNoTracking()
            //                             .Where(s => s.Id == subjectId)
            //                             .FirstOrDefaultAsync(cancellationToken);

            //if (subject == null)
            //    throw new SubjectNotFoundException(subjectId.Value);

            //QuestionExtention.CheckRoleAddQuestion(subject, userId, query.Role);

            //var pageIndex = query.PaginationRequest.PageIndex;
            //var pageSize = query.PaginationRequest.PageSize;
            //var totalCount = subject.QuestionIds.Count;

            //var questions = await dbContext.Questions
            //                               .AsNoTracking()
            //                               .Where(q => q.SubjectId == subjectId)
            //                               .Select(q => QuestionExtention.ConvertToQuestionDto(q))
            //                               .Skip((pageIndex - 1) * pageSize)
            //                               .Take(pageSize)
            //                               .ToListAsync(cancellationToken);

            //return new GetQuestionsBySubjectIdResult(new PaginationResult<QuestionBaseDto>(
            //    pageIndex,
            //    pageSize,
            //    totalCount,
            //    questions
            //    ));

            throw new NotImplementedException();
        }
    }
}
