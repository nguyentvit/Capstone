using Capstone.Domain.ChapterDomain.Models;
using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.ChapterDomain.Commands.AddChapter
{
    public class AddChapterHandler(IApplicationDbContext dbContext) : ICommandHandler<AddChapterCommand, AddChapterResult>
    {
        public async Task<AddChapterResult> Handle(AddChapterCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.UserId);
            var subjectId = SubjectId.Of(command.SubjectId);

            var subject = await dbContext.Subjects
                                         .Where(s => s.Id == subjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            SubjectExtention.CheckRole(subject, userId, command.Role);

            var chapter = Chapter.Of(
                ChapterTitle.Of(command.Title),
                ChapterOrder.Of(subject.ChapterIds.Count),
                subjectId
                );

            subject.AddChapterId(chapter.Id);
            dbContext.Chapters.Add(chapter);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddChapterResult(chapter.Id.Value);
        }
    }
}
