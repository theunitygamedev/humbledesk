using COI.Domain.Enums;

namespace COI.Application.QuestionSets.DTOs;

public class QuestionSetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AudienceType AudienceType { get; set; }
    public int Version { get; set; }
    public QuestionSetStatus Status { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<SectionDto> Sections { get; set; } = new();
}

public class SectionDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}

public class QuestionDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string TextHtml { get; set; } = string.Empty;
    public string? HelpHtml { get; set; }
    public QuestionType Type { get; set; }
    public bool Required { get; set; }
    public string? ConstraintsJson { get; set; }
    public string? Tags { get; set; }
    public List<OptionDto> Options { get; set; } = new();
}

public class OptionDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Order { get; set; }
}
