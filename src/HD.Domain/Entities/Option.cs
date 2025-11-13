using HD.Domain.Common;

namespace HD.Domain.Entities;

public class Option : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int Order { get; set; }

    // Navigation properties
    public Question Question { get; set; } = null!;
}
