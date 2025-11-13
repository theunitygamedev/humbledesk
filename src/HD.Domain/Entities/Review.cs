using HD.Domain.Common;
using HD.Domain.Enums;

namespace HD.Domain.Entities;

public class Review : BaseEntity, IAuditableEntity
{
    public Guid SubmissionId { get; set; }
    public ReviewStatus Status { get; set; }
    public string ReviewerId { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Submission Submission { get; set; } = null!;
}
