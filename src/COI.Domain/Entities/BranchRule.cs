using COI.Domain.Common;

namespace COI.Domain.Entities;

public class BranchRule : BaseEntity
{
    public Guid SourceQuestionId { get; set; }
    public string Operator { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public Guid? TargetQuestionId { get; set; }
    public Guid? TargetSectionId { get; set; }

    // Navigation properties
    public Question SourceQuestion { get; set; } = null!;
    public Question? TargetQuestion { get; set; }
    public Section? TargetSection { get; set; }
}
