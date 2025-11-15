using HD.Application.Tickets.Commands.CreateTicket;
using HD.Application.Tickets.Commands.DeleteTicket;
using HD.Application.Tickets.Commands.UpdateTicket;
using HD.Application.Tickets.DTOs;
using HD.Application.Tickets.Queries.GetTicketById;
using HD.Application.Tickets.Queries.GetTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all tickets for the current tenant
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TicketDto>>> GetTickets()
    {
        var tickets = await _mediator.Send(new GetTicketsQuery());
        return Ok(tickets);
    }

    /// <summary>
    /// Get a specific ticket by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> GetTicketById(Guid id)
    {
        try
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery(id));
            return Ok(ticket);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new ticket
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketDto dto)
    {
        var ticket = await _mediator.Send(new CreateTicketCommand(dto));
        return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
    }

    /// <summary>
    /// Update an existing ticket
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> UpdateTicket(Guid id, [FromBody] UpdateTicketDto dto)
    {
        try
        {
            var ticket = await _mediator.Send(new UpdateTicketCommand(id, dto));
            return Ok(ticket);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete a ticket
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteTicketCommand(id));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
