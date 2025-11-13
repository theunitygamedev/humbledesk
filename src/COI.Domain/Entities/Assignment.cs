using COI.Domain.Common;
using COI.Domain.Enums;

namespace COI.Domain.Entities;

public class Assignment : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid QuestionSetId { get; set; }
    public Guid? CycleId { get; set; }
    public DateTime DueDate { get; set; }
    public AssignmentStatus Status { get; set; }

    // Navigation properties
    public QuestionSet QuestionSet { get; set; } = null!;
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
