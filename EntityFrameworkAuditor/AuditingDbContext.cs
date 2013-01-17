using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using EntityFrameworkAuditor.ChangeEvents;

namespace EntityFrameworkAuditor
{
    public class AuditingDbContext : DbContext
    {
        public AuditingDbContext(string connectionString) : base(connectionString)
        {
        }

        public event EventHandler<DataChangedEventArgs> DataChanged;

        public override int SaveChanges()
        {
            var entityChanges = EntityChanges();
            var relationshipChanges = RelationshipChanges();

            var auditRecords = entityChanges.Cast<IAuditRecord>().Union(relationshipChanges.Cast<IAuditRecord>());
            RaiseDataChanged(auditRecords);

            var result = base.SaveChanges();
            return result;
        }

        private void RaiseDataChanged(IEnumerable<IAuditRecord> auditRecords)
        {
            var handler = DataChanged;
            if (handler == null) return;

            var e = new DataChangedEventArgs(auditRecords.ToArray());
            handler(this, e);
        }

        private IEnumerable<EntityChange> EntityChanges()
        {
            foreach (var e in ChangeTracker.Entries())
            {
                var internalEntry = e.GetType().GetField("_internalEntityEntry", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(e);
                var objectStateEntry = internalEntry.GetType().GetField("_stateEntry", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(internalEntry);
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
                var sourceKey = (EntityKey) (ose.State == EntityState.Added ? ose.CurrentValues[0] : ose.OriginalValues[0]);
                var targetKey = (EntityKey) (ose.State == EntityState.Added ? ose.CurrentValues[1] : ose.OriginalValues[1]);
                var source = objectContext.GetObjectByKey(sourceKey);
                var target = objectContext.GetObjectByKey(targetKey);

                yield return new RelationshipChange(sourceKey, targetKey, source, target, ose.EntitySet, ose.State);
            }
        }
    }
}