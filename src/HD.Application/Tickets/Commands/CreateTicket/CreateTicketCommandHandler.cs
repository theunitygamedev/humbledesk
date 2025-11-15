using AutoMapper;
using HD.Application.Common.Interfaces;
using HD.Application.Tickets.DTOs;
using HD.Domain.Entities;
using HD.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HD.Application.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateTicketCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated");
        var tenantId = _currentUserService.TenantId
            ?? throw new UnauthorizedAccessException("User does not belong to a tenant");

        var ticket = _mapper.Map<Ticket>(request.Dto);
        ticket.TenantId = tenantId;
        ticket.CreatedByUserId = userId;
        ticket.Status = TicketStatus.New;
        ticket.CreatedBy = _currentUserService.UserEmail ?? "System";
        ticket.CreatedAt = DateTime.UtcNow;

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with navigation properties
        var createdTicket = await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstAsync(t => t.Id == ticket.Id, cancellationToken);

        return _mapper.Map<TicketDto>(createdTicket);
    }
}
