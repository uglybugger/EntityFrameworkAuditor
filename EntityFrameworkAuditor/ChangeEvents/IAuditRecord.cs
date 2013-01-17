using System.Collections.Generic;
using System.Data;

namespace EntityFrameworkAuditor.ChangeEvents
{
    public interface IAuditRecord
    {
        IEnumerable<EntityKey> PertainsTo { get; }
    }
}