using AutoMapper;
using HD.Application.Common.Interfaces;
using HD.Application.Tickets.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HD.Application.Tickets.Commands.UpdateTicket;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTicketCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<TicketDto> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId
            ?? throw new UnauthorizedAccessException("User does not belong to a tenant");

        var ticket = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.TenantId == tenantId, cancellationToken)
            ?? throw new KeyNotFoundException($"Ticket with ID {request.Id} not found");

        // Update only provided fields
        if (request.Dto.Title != null)
            ticket.Title = request.Dto.Title;

        if (request.Dto.Description != null)
            ticket.Description = request.Dto.Description;

        if (request.Dto.Status.HasValue)
            ticket.Status = request.Dto.Status.Value;

        if (request.Dto.Priority.HasValue)
            ticket.Priority = request.Dto.Priority.Value;

        if (request.Dto.Category != null)
            ticket.Category = request.Dto.Category;

        if (request.Dto.AssignedToUserId.HasValue)
            ticket.AssignedToUserId = request.Dto.AssignedToUserId.Value;

        ticket.UpdatedBy = _currentUserService.UserEmail ?? "System";
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with updated navigation properties
        var updatedTicket = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstAsync(t => t.Id == ticket.Id, cancellationToken);

        return _mapper.Map<TicketDto>(updatedTicket);
    }
}
