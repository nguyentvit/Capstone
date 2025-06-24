using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.QuestionDomain.Queries.GetQuestionVersionsByQuestionId
{
    public class GetQuestionVersionsByQuestionIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetQuestionVersionsByQuestionIdQuery, GetQuestionVersionsByQuestionIdResult>
    {
        public async Task<GetQuestionVersionsByQuestionIdResult> Handle(GetQuestionVersionsByQuestionIdQuery query, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(query.QuestionId);

            var question = await dbContext.Questions
                                          .AsNoTracking()
                                          .Where(q => q.Id == questionId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(query.QuestionId);

            if (question.UserId.Value != query.UserId)
                throw new AccessNotAllowException();

            var rootQuestionId = question.RootId;

            var questionVersion = await dbContext.Questions
                                                 .AsNoTracking()
                                                 .Where(q => q.Id != questionId && q.RootId == rootQuestionId)
                                                 .OrderByDescending(q => q.CreatedAt)
                                                 .Select(q => new GetQuestionVersionsByQuestionIdDto(QuestionExtension.ConvertToQuestionDto(q)))
                                                 .ToListAsync(cancellationToken);

            return new GetQuestionVersionsByQuestionIdResult(questionVersion);
        }
    }
}
