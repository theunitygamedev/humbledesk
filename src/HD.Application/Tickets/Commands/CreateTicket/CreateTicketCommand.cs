using HD.Application.Tickets.DTOs;
using MediatR;

namespace HD.Application.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(CreateTicketDto Dto) : IRequest<TicketDto>;
