using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Entities;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.ValueObjects;

namespace Capstone.Application.QuestionDomain.Commands.AddSingleChoiceQuestion
{
    public class AddSingleChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddSingleChoiceQuestionCommand, AddSingleChoiceQuestionResult>
    {
        public async Task<AddSingleChoiceQuestionResult> Handle(AddSingleChoiceQuestionCommand command, CancellationToken cancellationToken)
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

                var singleChoiceQuestion = AddSingleChoiceQuestionCommandToSingleChoiceQuestion(command);

                chapter.AddQuestionId(singleChoiceQuestion.Id);
                dbContext.SingleChoiceQuestions.Add(singleChoiceQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddSingleChoiceQuestionResult(singleChoiceQuestion.Id.Value);
            }
            else
            {
                var singleChoiceQuestion = AddSingleChoiceQuestionCommandToSingleChoiceQuestion(command);
                dbContext.SingleChoiceQuestions.Add(singleChoiceQuestion);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new AddSingleChoiceQuestionResult(singleChoiceQuestion.Id.Value);
            }

                throw new NotImplementedException();
        }
        private static SingleChoiceQuestion AddSingleChoiceQuestionCommandToSingleChoiceQuestion(AddSingleChoiceQuestionCommand command)
        {
            var choiceEntities = command.Choices.Select(c => ChoiceSingle.Of(ChoiceSingleContent.Of(c))).ToList();
            var correctAnswerId = choiceEntities[command.CorrectAnswerIndex].Id;

            return SingleChoiceQuestion.Of(
                QuestionTitle.Of(command.Title),
                QuestionContent.Of(command.Content),
                (QuestionDifficulty)command.Difficulty,
                UserId.Of(command.UserId),
                (command.ChapterId != null) ? ChapterId.Of((Guid)command.ChapterId) : null,
                choiceEntities,
                correctAnswerId
                );
        }
    }
}
