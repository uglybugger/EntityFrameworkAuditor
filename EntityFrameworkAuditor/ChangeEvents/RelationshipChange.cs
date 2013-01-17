using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Linq;

namespace EntityFrameworkAuditor.ChangeEvents
{
    public class RelationshipChange : IAuditRecord
    {
        private readonly EntityKey _sourceKey;
        private readonly EntityKey _targetKey;
        private readonly object _source;
        private readonly object _target;
        private readonly EntitySetBase _entitySet;
        private readonly EntityState _state;

        public RelationshipChange(EntityKey sourceKey, EntityKey targetKey, object source, object target, EntitySetBase entitySet, EntityState state)
        {
            _sourceKey = sourceKey;
            _targetKey = targetKey;
            _source = source;
            _target = target;
            _entitySet = entitySet;
            _state = state;
        }

        public override string ToString()
        {
            var s = string.Format("{0} was {1} {2}'s {3} collection", _target, StateChangeDescription(), _source, CollectionDescription());
            return s;
        }

        private string CollectionDescription()
        {
            var tokens = _entitySet.Name.Split('_');
            var description = string.Join("_", tokens.Skip(1));
            return description;
        }

        private string StateChangeDescription()
        {
            return _state == EntityState.Added ? "added to" : "removed from";
        }

        public IEnumerable<EntityKey> PertainsTo
        {
            get
            {
                yield return _sourceKey;
                yield return _targetKey;
            }
        }
    }
}