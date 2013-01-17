using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFrameworkAuditor.App.Domain.Entities;

namespace EntityFrameworkAuditor.App
{
    public class DomainContext : DbContext
    {
        public DomainContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public override int SaveChanges()
        {
            var entityChanges = EntityChanges();
            var relationshipChanges = RelationshipChanges();

            var changes = entityChanges
                .Union(relationshipChanges.Cast<object>())
                .Select(c => c.ToString())
                .ToArray();

            foreach (var change in changes) Console.WriteLine(change);
            var result = base.SaveChanges();
            return result;
        }

        private IEnumerable<EntityChange> EntityChanges()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            foreach (DbEntityEntry e in ChangeTracker.Entries())
            {
                var internalEntry = e.GetType().GetField("_internalEntityEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(e);
                var objectStateEntry = internalEntry.GetType().GetField("_stateEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(internalEntry);
                var entityKey = (EntityKey) objectStateEntry.GetType().GetProperty("EntityKey").GetValue(objectStateEntry);
                yield return new EntityChange(entityKey, e);
            }
        }

        private IEnumerable<RelationshipChange> RelationshipChanges()
        {
            var objectContext = ((IObjectContextAdapter) this).ObjectContext;

            var changedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted)
                                                  .Where(e => e.IsRelationship)
                                                  .ToArray();


            foreach (var ose in changedRelationships)
            {
                var sourceKey = (EntityKey)(ose.State == EntityState.Added ? ose.CurrentValues[0] : ose.OriginalValues[0]);
                var targetKey = (EntityKey)(ose.State == EntityState.Added ? ose.CurrentValues[1] : ose.OriginalValues[1]);
                var source = objectContext.GetObjectByKey(sourceKey);
                var target = objectContext.GetObjectByKey(targetKey);

                yield return new RelationshipChange(sourceKey, targetKey, source, target, ose.EntitySet, ose.State);
            }

        }
    }
}