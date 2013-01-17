using System;
using EntityFrameworkAuditor.ChangeEvents;

namespace EntityFrameworkAuditor
{
    public class DataChangedEventArgs : EventArgs
    {
        private readonly IAuditRecord[] _auditRecords;

        public DataChangedEventArgs(IAuditRecord[] auditRecords)
        {
            _auditRecords = auditRecords;
        }

        public IAuditRecord[] AuditRecords
        {
            get { return _auditRecords; }
        }
    }
}