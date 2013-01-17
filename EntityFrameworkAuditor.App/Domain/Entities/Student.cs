using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkAuditor.App.Domain.Entities
{
    public class Student : IAggregateRoot
    {
        protected Student()
        {
        }

        public Student(string firstName, string lastName)
        {
            Id = Guid.NewGuid(); //FIXME hack
            FirstName = firstName;
            LastName = lastName;
            EnrolledInSubjects = new HashSet<Subject>();
        }

        public void EnrolIn(Subject subject)
        {
            subject.AcceptEnrolmentFor(this);
            EnrolledInSubjects.Add(subject);
        }

        [Key]
        public virtual Guid Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual ICollection<Subject> EnrolledInSubjects { get; protected set; }

        public override string ToString()
        {
            return "{0} {1}".FormatWith(FirstName, LastName);
        }

        public void CancelEnrolmentIn(Subject subject)
        {
            subject.CancelEnrolmentFor(this);
            EnrolledInSubjects.Remove(subject);
        }
    }
}