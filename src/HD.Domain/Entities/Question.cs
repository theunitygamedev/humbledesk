using HD.Domain.Common;
using HD.Domain.Enums;

namespace HD.Domain.Entities;

public class Question : BaseEntity
{
    public Guid SectionId { get; set; }
    public int Order { get; set; }
    public string TextHtml { get; set; } = string.Empty;
    public string? HelpHtml { get; set; }
    public QuestionType Type { get; set; }
    public bool Required { get; set; }
    public string? ConstraintsJson { get; set; }
    public string? Tags { get; set; }
    public string? VisibilityRuleJson { get; set; }

    // Navigation properties
    public Section Section { get; set; } = null!;
    public ICollection<Option> Options { get; set; } = new List<Option>();
    public ICollection<BranchRule> SourceBranchRules { get; set; } = new List<BranchRule>();
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
