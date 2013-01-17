using System;
using System.Linq;
using EntityFrameworkAuditor.App.Domain.Entities;

namespace EntityFrameworkAuditor.App
{
    public class Program
    {
        public static void Main()
        {
            using (var context = CreateContext())
            {
                foreach (var student in context.Students.ToArray()) context.Students.Remove(student);
                foreach (var subject in context.Subjects.ToArray()) context.Subjects.Remove(subject);
                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var fred = new Student("Fred", "Flintstone");
                var barney = new Student("Barney", "Rubble");
                context.Students.Add(fred);
                context.Students.Add(barney);

                var law = new Subject("Law");

                context.Subjects.Add(law);

                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var fred = context.Students.First(s => s.FirstName == "Fred");
                var law = context.Subjects.Single();
                fred.EnrolIn(law);
                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var barney = context.Students.First(s => s.FirstName == "Barney");
                var law = context.Subjects.Single();
                barney.EnrolIn(law);
                context.SaveChanges();
            }

            using (var context = CreateContext())
            {
                var fred = context.Students.First(s => s.FirstName == "Fred");
                var law = context.Subjects.Single();
                fred.CancelEnrolmentIn(law);
                context.SaveChanges();
            }

            Console.ReadKey();
        }

        private static DomainContext CreateContext()
        {
            return new DomainContext(@"Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName=C:\Users\andrewh\EntityFrameworkAuditor.mdf");
        }
    }
}