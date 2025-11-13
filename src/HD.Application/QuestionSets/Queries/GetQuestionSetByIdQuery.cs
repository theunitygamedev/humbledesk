using HD.Application.Common.Interfaces;
using HD.Application.QuestionSets.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HD.Application.QuestionSets.Queries;

public record GetQuestionSetByIdQuery(Guid Id) : IRequest<QuestionSetDto?>;

public class GetQuestionSetByIdQueryHandler : IRequestHandler<GetQuestionSetByIdQuery, QuestionSetDto?>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionSetByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionSetDto?> Handle(GetQuestionSetByIdQuery request, CancellationToken cancellationToken)
    {
        var questionSet = await _context.QuestionSets
            .Include(qs => qs.Sections)
                .ThenInclude(s => s.Questions)
                    .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(qs => qs.Id == request.Id, cancellationToken);

        if (questionSet == null)
            return null;

        return new QuestionSetDto
        {
            Id = questionSet.Id,
            Name = questionSet.Name,
            AudienceType = questionSet.AudienceType,
            Version = questionSet.Version,
            Status = questionSet.Status,
            CreatedBy = questionSet.CreatedBy,
            CreatedAt = questionSet.CreatedAt,
            Sections = questionSet.Sections.OrderBy(s => s.Order).Select(s => new SectionDto
            {
                Id = s.Id,
                Order = s.Order,
                Title = s.Title,
                Description = s.Description,
                Questions = s.Questions.OrderBy(q => q.Order).Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Order = q.Order,
                    TextHtml = q.TextHtml,
                    HelpHtml = q.HelpHtml,
                    Type = q.Type,
                    Required = q.Required,
                    ConstraintsJson = q.ConstraintsJson,
                    Tags = q.Tags,
                    Options = q.Options.OrderBy(o => o.Order).Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Value = o.Value,
                        Label = o.Label,
                        Order = o.Order
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}
