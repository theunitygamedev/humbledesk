using COI.Domain.Common;

namespace COI.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid SubmissionId { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    // Navigation properties
    public Submission Submission { get; set; } = null!;
}
