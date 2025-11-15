using HD.Domain.Enums;

namespace HD.Application.Tickets.DTOs;

public class TicketDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string Category { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Additional display fields
    public string? AssignedToUserName { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
}
