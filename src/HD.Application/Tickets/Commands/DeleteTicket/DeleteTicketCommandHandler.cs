using HD.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HD.Application.Tickets.Commands.DeleteTicket;

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTicketCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId
            ?? throw new UnauthorizedAccessException("User does not belong to a tenant");

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.TenantId == tenantId, cancellationToken)
            ?? throw new KeyNotFoundException($"Ticket with ID {request.Id} not found");

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
