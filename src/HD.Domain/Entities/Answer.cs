using HD.Domain.Common;

namespace HD.Domain.Entities;

public class Answer : BaseEntity
{
    public Guid SubmissionId { get; set; }
    public Guid QuestionId { get; set; }
    public string ValueJson { get; set; } = string.Empty;

    // Navigation properties
    public Submission Submission { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
