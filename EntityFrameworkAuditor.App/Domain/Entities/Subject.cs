using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAuditor.App.Domain.Entities
{
    public class Subject : IAggregateRoot
    {
        protected Subject()
        {
        }

        public Subject(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            EnrolledStudents = new HashSet<Student>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<Student> EnrolledStudents { get; set; }

        public void AcceptEnrolmentFor(Student student)
        {
            EnrolledStudents.Add(student);
        }

        public void CancelEnrolmentFor(Student student)
        {
            EnrolledStudents.Remove(student);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}