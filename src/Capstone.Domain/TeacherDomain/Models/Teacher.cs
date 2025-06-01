using Capstone.Domain.TeacherDomain.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.TeacherDomain.Models
{
    public class Teacher : User
    {
        public TeacherId TeacherId { get; private set; } = default!;
        private Teacher() { }
        private Teacher(UserId id, UserName userName, Email? email, PhoneNumber? phone, FileVO? avatar, TeacherId teacherId) : base(id, userName, email, phone, avatar)
        {
            TeacherId = teacherId;
        }
        public static Teacher Of(UserName userName, Email? email, PhoneNumber? phone, FileVO? avatar, TeacherId teacherId)
        {
            return new Teacher(
                UserId.Of(Guid.NewGuid()),
                userName,
                email,
                phone,
                avatar,
                teacherId
                );
        }
    }
}
