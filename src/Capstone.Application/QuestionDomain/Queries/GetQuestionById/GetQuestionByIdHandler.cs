using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.QuestionDomain.Queries.GetQuestionById
{
    public class GetQuestionByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetQuestionByIdQuery, GetQuestionByIdResult>
    {
        public async Task<GetQuestionByIdResult> Handle(GetQuestionByIdQuery query, CancellationToken cancellationToken)
        {
            var questionId = QuestionId.Of(query.QuestionId);

            var question = await dbContext.Questions
                                          .AsNoTracking()
                                          .Where(q => q.Id == questionId)
                                          .Select(q => QuestionExtension.ConvertToQuestionDto(q))
                                          .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(questionId.Value);


            if (question.UserId != query.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào câu hỏi này");

            return new GetQuestionByIdResult(question);
        }
    }
}
