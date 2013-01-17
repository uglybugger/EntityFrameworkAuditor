using System;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace EntityFrameworkAuditor
{
    public static class Auditor
    {
        public static string ToAuditMessage(this DbEntityEntry e)
        {
            switch (e.State)
            {
                case EntityState.Added:
                    return e.ToAddedAuditMessage();
                case EntityState.Deleted:
                    return e.ToDeletedMessage();
                case EntityState.Modified:
                    return e.ToModifiedMessage();
                default:
                    // don't care
                    return null;
            }
        }

        public static string ToAddedAuditMessage(this DbEntityEntry e)
        {
            var message = "Added " + e.Entity.GetType().Name;
            foreach (var pn in e.CurrentValues.PropertyNames)
            {
                var v = e.CurrentValues.GetValue<object>(pn).ToString();
                message += "\r\n" + pn + ": " + v;
            }
            return message;
        }

        public static string ToDeletedMessage(this DbEntityEntry e)
        {
            throw new NotImplementedException();
        }

        public static string ToModifiedMessage(this DbEntityEntry e)
        {
            var message = "Modified " + e.Entity.GetType().Name;
            foreach (var pn in e.CurrentValues.PropertyNames)
            {
                var originalValue = e.OriginalValues.GetValue<object>(pn).ToString();
                var currentValue = e.CurrentValues.GetValue<object>(pn).ToString();
                if (originalValue == currentValue) continue;
                message += "\r\n" + pn + ": " + currentValue;
            }
            return message;
        }
    }
}