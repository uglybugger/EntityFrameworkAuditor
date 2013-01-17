using System;
using EntityFrameworkAuditor.ChangeEvents;

namespace EntityFrameworkAuditor
{
    public class AuditableActionsOccurredEventArgs : EventArgs
    {
        private readonly IAuditRecord[] _auditRecords;

        public AuditableActionsOccurredEventArgs(IAuditRecord[] auditRecords)
        {
            _auditRecords = auditRecords;
        }

        public IAuditRecord[] AuditRecords
        {
            get { return _auditRecords; }
        }
    }
}