using Capstone.Domain.StudentDomain.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.StudentDomain.Models
{
    public class Student : User
    {
        public StudentId StudentId { get; private set; } = default!;
        private Student() { }
        private Student(UserId id, UserName userName, Email? email, PhoneNumber? phone, FileVO? avatar, StudentId studentId) : base(id, userName, email, phone, avatar)
        {
            StudentId = studentId;
        }
        public static Student Of(UserName userName, Email? email, PhoneNumber? phone, FileVO? avatar, StudentId studentId)
        {
            return new Student(
                UserId.Of(Guid.NewGuid()),
                userName,
                email,
                phone,
                avatar,
                studentId
                );
        }
    }
}
