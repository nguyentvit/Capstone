using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;

namespace Capstone.Application.QuestionDomain.Commands.AddEssayQuestion
{
    public class AddEssayQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddEssayQuestionCommand, AddEssayQuestionResult>
    {
        public async Task<AddEssayQuestionResult> Handle(AddEssayQuestionCommand command, CancellationToken cancellationToken)
        {
            if (command.ChapterId != null)
            {
                var userId = UserId.Of(command.UserId);
                var chapterId = ChapterId.Of(command.ChapterId.Value);

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

                var essayQuestion = AddEssayQuestionCommandToEssayQuestion(command);

                chapter.AddQuestionId(essayQuestion.Id);
                dbContext.EssayQuestions.Add(essayQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddEssayQuestionResult(essayQuestion.Id.Value);
            }
            else
            {
                var essayQuestion = AddEssayQuestionCommandToEssayQuestion(command);
                dbContext.EssayQuestions.Add(essayQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddEssayQuestionResult(essayQuestion.Id.Value);
            }   
        }
        private static EssayQuestion AddEssayQuestionCommandToEssayQuestion(AddEssayQuestionCommand command)
        {
            return EssayQuestion.Of(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                UserId.Of(command.UserId),
                (command.ChapterId != null) ? ChapterId.Of((Guid)command.ChapterId) : null
                );
        }
    }
}
