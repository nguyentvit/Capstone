using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.AddTrueFalseQuestion
{
    public class AddTrueFalseQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddTrueFalseQuestionCommand, AddTrueFalseQuestionResult>
    {
        public async Task<AddTrueFalseQuestionResult> Handle(AddTrueFalseQuestionCommand command, CancellationToken cancellationToken)
        {
            if (command.ChapterId != null)
            {
                var userId = UserId.Of(command.UserId);
                var chapterId = ChapterId.Of((Guid)command.ChapterId);

                var chapter = await dbContext.Chapters
                                             .Where(s => s.Id == chapterId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (chapter == null)
                    throw new ChapterNotFoundException(chapterId.Value);

                var subject = await dbContext.Subjects
                                             .AsNoTracking()
                                             .Where(s => s.Id == chapter.SubjectId)
                                             .FirstOrDefaultAsync(cancellationToken);

                if (subject == null)
                    throw new SubjectNotFoundException(chapter.SubjectId.Value);

                SubjectExtention.CheckRole(subject, userId, command.Role);

                var trueFalseQuestion = AddTrueFalseQuestionCommandToTrueFalseQuestion(command);
                chapter.AddQuestionId(trueFalseQuestion.Id);

                dbContext.TrueFalseQuestions.Add(trueFalseQuestion);
                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddTrueFalseQuestionResult(trueFalseQuestion.Id.Value);

            }
            else
            {
                var trueFalseQuestion = AddTrueFalseQuestionCommandToTrueFalseQuestion(command);
                dbContext.TrueFalseQuestions.Add(trueFalseQuestion);
                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddTrueFalseQuestionResult(trueFalseQuestion.Id.Value);
            }
        }
        private static TrueFalseQuestion AddTrueFalseQuestionCommandToTrueFalseQuestion(AddTrueFalseQuestionCommand command)
        {
            return TrueFalseQuestion.Of(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                UserId.Of(command.UserId),
                (command.ChapterId != null) ? ChapterId.Of((Guid)command.ChapterId) : null,
                IsTrueAnswer.Of(command.IsTrueAnswer)
                );
        }
    }
}
