using HD.Application.Tickets.DTOs;
using MediatR;

namespace HD.Application.Tickets.Queries.GetTickets;

public record GetTicketsQuery : IRequest<List<TicketDto>>;
