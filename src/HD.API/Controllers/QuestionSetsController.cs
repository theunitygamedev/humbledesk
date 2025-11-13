using HD.Application.QuestionSets.Commands;
using HD.Application.QuestionSets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionSetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionSetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionSetByIdQuery(id), cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "LegalAdmin")]
    public async Task<IActionResult> Create(CreateQuestionSetCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
