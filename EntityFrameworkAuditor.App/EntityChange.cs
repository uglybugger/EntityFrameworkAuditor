using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace EntityFrameworkAuditor.App
{
    public class EntityChange : IAuditRecord
    {
        private readonly EntityKey _entityKey;
        private readonly DbEntityEntry _dbEntityEntry;

        public EntityChange(EntityKey entityKey, DbEntityEntry dbEntityEntry)
        {
            _entityKey = entityKey;
            _dbEntityEntry = dbEntityEntry;
        }

        public override string ToString()
        {
            return _dbEntityEntry.ToAuditMessage();
        }

        public IEnumerable<EntityKey> PertainsTo
        {
            get { yield return _entityKey; }
        }
    }
}