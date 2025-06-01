using Capstone.Domain.ClassDomain.Entities;
using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Domain.ClassDomain.Models
{
    public class Class : Aggregate<ClassId>
    {
        private readonly List<ClassStudent> _students = new();
        public IReadOnlyList<ClassStudent> Students => _students.AsReadOnly();
        public SubjectId SubjectId { get; } = default!;
        public ClassName Name { get; set; } = default!;
        private Class() { }
        private Class(SubjectId subjectId, ClassName name)
        {
            Id = ClassId.Of(Guid.NewGuid());
            SubjectId = subjectId;
            Name = name;
        }
        public static Class Of(SubjectId subjectId, ClassName name)
        {
            return new Class(subjectId, name);
        }
        public void AddStudent(ClassStudent student)
        {
            _students.Add(student);
        }
    }
}
