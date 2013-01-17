using System;

namespace EntityFrameworkAuditor.App.Domain.Entities
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}