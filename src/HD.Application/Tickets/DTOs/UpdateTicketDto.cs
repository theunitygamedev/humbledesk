using HD.Domain.Enums;

namespace HD.Application.Tickets.DTOs;

public class UpdateTicketDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TicketStatus? Status { get; set; }
    public TicketPriority? Priority { get; set; }
    public string? Category { get; set; }
    public Guid? AssignedToUserId { get; set; }
}
