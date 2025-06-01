using Capstone.Domain.ChapterDomain.Models;
using Capstone.Domain.ClassDomain.Models;
using Capstone.Domain.ExamModule.Models;
using Capstone.Domain.ExamSessionModule.Models;
using Capstone.Domain.ExamTemplateModule.Models;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;
using Capstone.Domain.SubjectDomain.Models;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Capstone.Application.Interface;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> ApplicationUsers { get; }
    DbSet<User> AppUsers { get; }
    DbSet<Teacher> Teachers { get; }
    DbSet<Student> Students { get; }
    DbSet<PersistedGrant> PersistedGrantsDb { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<SystemSubject> SystemSubjects { get; }
    DbSet<TeacherSubject> TeacherSubjects { get; }
    DbSet<Class> Classes { get; }
    DbSet<Question> Questions { get; }
    DbSet<TrueFalseQuestion> TrueFalseQuestions { get; }
    DbSet<SingleChoiceQuestion> SingleChoiceQuestions { get; }
    DbSet<MultiChoiceQuestion> MultiChoiceQuestions { get; }
    DbSet<MatchingQuestion> MatchingQuestions { get; }
    DbSet<Chapter> Chapters { get; }
    DbSet<ExamTemplate> ExamTemplates { get; }
    DbSet<Exam> Exams { get; }
    DbSet<ExamSession> ExamSessions { get; }
    DbSet<EssayQuestion> EssayQuestions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);
}