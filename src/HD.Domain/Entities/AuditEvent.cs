using HD.Domain.Common;

namespace HD.Domain.Entities;

public class AuditEvent : BaseEntity
{
    public string ActorId { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? PayloadJson { get; set; }
}
