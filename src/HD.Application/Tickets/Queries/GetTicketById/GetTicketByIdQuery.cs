using HD.Application.Tickets.DTOs;
using MediatR;

namespace HD.Application.Tickets.Queries.GetTicketById;

public record GetTicketByIdQuery(Guid Id) : IRequest<TicketDto>;
