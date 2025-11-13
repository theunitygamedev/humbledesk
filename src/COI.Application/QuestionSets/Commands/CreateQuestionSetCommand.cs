using COI.Application.Common.Interfaces;
using COI.Domain.Entities;
using COI.Domain.Enums;
using MediatR;

namespace COI.Application.QuestionSets.Commands;

public record CreateQuestionSetCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public AudienceType AudienceType { get; init; }
}

public class CreateQuestionSetCommandHandler : IRequestHandler<CreateQuestionSetCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateQuestionSetCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateQuestionSetCommand request, CancellationToken cancellationToken)
    {
        var questionSet = new QuestionSet
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            AudienceType = request.AudienceType,
            Version = 1,
            Status = QuestionSetStatus.Draft,
            CreatedBy = _currentUser.UserId ?? "system",
            CreatedAt = DateTime.UtcNow
        };

        _context.QuestionSets.Add(questionSet);
        await _context.SaveChangesAsync(cancellationToken);

        return questionSet.Id;
    }
}
