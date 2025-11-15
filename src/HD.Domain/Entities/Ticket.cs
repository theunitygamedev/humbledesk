using HD.Domain.Common;
using HD.Domain.Enums;

namespace HD.Domain.Entities;

public class Ticket : BaseEntity, IAuditableEntity
{
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public string Category { get; set; } = string.Empty;

    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    // IAuditableEntity
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? AssignedToUser { get; set; }
}
