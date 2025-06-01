using System.Reflection;
using Capstone.Application.Interface;
using Capstone.Domain.ChapterDomain.Models;
using Capstone.Domain.ClassDomain.Models;
using Capstone.Domain.ExamModule.Models;
using Capstone.Domain.ExamSessionModule.Models;
using Capstone.Domain.ExamTemplateModule.Models;
using Capstone.Domain.Identity.Models;
using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;
using Capstone.Domain.StudentDomain.Models;
using Capstone.Domain.SubjectDomain.Models;
using Capstone.Domain.TeacherDomain.Models;
using Capstone.Domain.UserAccess.Models;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Capstone.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<User> AppUsers => Set<User>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<PersistedGrant> PersistedGrantsDb => Set<PersistedGrant>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<SystemSubject> SystemSubjects => Set<SystemSubject>();
    public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<TrueFalseQuestion> TrueFalseQuestions => Set<TrueFalseQuestion>();
    public DbSet<SingleChoiceQuestion> SingleChoiceQuestions => Set<SingleChoiceQuestion>();
    public DbSet<MultiChoiceQuestion> MultiChoiceQuestions => Set<MultiChoiceQuestion>();
    public DbSet<MatchingQuestion> MatchingQuestions => Set<MatchingQuestion>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<ExamTemplate> ExamTemplates => Set<ExamTemplate>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<ExamSession> ExamSessions => Set<ExamSession>();
    public DbSet<EssayQuestion> EssayQuestions => Set<EssayQuestion>();
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        await transaction.RollbackAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}