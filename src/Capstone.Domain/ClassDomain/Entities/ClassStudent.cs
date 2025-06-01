using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.ClassDomain.Entities
{
    public class ClassStudent : Entity<ClassStudentId>
    {
        public UserId StudentId { get; } = default!;
        private ClassStudent() { }
        private ClassStudent(UserId studentId)
        {
            Id = ClassStudentId.Of(Guid.NewGuid());
            StudentId = studentId;
        }
        public static ClassStudent Of(UserId studentId)
        {
            return new ClassStudent(studentId);
        }
    }
}
