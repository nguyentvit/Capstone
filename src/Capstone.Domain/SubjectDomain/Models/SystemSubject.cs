using Capstone.Domain.SubjectDomain.Entities;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Domain.SubjectDomain.Models
{
    public class SystemSubject : Subject
    {
        //private readonly List<TeacherQuestionRequest> _teacherQuestionRequest = new();
        //public IReadOnlyList<TeacherQuestionRequest> TeacherQuestionRequest => _teacherQuestionRequest.AsReadOnly();
        private SystemSubject() { }
        private SystemSubject(SubjectName subjectName) : base(subjectName)
        {

        }
        public static SystemSubject Of(SubjectName subjectName)
        {
            return new SystemSubject(subjectName);
        }
    }
}
