using HD.Domain.Enums;

namespace HD.Application.Tickets.DTOs;

public class CreateTicketDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public string Category { get; set; } = string.Empty;
}
