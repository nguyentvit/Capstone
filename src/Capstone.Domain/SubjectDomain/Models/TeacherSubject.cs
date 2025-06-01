using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.SubjectDomain.Models
{
    public class TeacherSubject : Subject
    {
        private readonly List<ClassId> _classIds = new();
        public IReadOnlyList<ClassId> ClassIds => _classIds.AsReadOnly();
        public UserId OwnerId { get; } = default!;
        private TeacherSubject() { }
        private TeacherSubject(SubjectName subjectName, UserId ownerId) : base(subjectName)
        {
            OwnerId = ownerId;
        }
        public static TeacherSubject Of(SubjectName subjectName, UserId ownerId)
        {
            return new TeacherSubject(subjectName, ownerId);
        }
        public void AddClass(ClassId classId)
        {
            _classIds.Add(classId);
        }
    }
}
