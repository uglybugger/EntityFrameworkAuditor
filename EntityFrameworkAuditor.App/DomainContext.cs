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
            DataChanged += OnDataChanged;
        }

        private void OnDataChanged(object sender, DataChangedEventArgs e)
        {
            var changes = e.AuditRecords
                           .Select(c => c.ToString())
                           .ToArray();

            foreach (var change in changes) Console.WriteLine(change);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }
}