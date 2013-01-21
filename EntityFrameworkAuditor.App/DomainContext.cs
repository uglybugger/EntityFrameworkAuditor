using System;
using System.Data.Entity;
using System.Linq;
using EntityFrameworkAuditor.App.Domain.Entities;

namespace EntityFrameworkAuditor.App
{
    public class DomainContext : AuditingDbContext
    {
        public DomainContext(string connectionString)
            : base(connectionString)
        {
            AuditableActionsOccurred += OnAuditableActionsOccurred;
        }

        private void OnAuditableActionsOccurred(object sender, AuditableActionsOccurredEventArgs e)
        {
            var changes = e.AuditRecords
                           .Select(c => c.ToString())
                           .ToArray();

            foreach (var change in changes)
            {
                Console.WriteLine(change);
                Console.WriteLine();
            }
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }
}