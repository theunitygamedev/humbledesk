using MediatR;

namespace HD.Application.Tickets.Commands.DeleteTicket;

public record DeleteTicketCommand(Guid Id) : IRequest<Unit>;
