using HD.Application.Tickets.DTOs;
using MediatR;

namespace HD.Application.Tickets.Commands.UpdateTicket;

public record UpdateTicketCommand(Guid Id, UpdateTicketDto Dto) : IRequest<TicketDto>;
