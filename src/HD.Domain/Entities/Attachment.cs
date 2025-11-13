using HD.Domain.Common;

namespace HD.Domain.Entities;

public class Attachment : BaseEntity
{
    public Guid SubmissionId { get; set; }
    public Guid? QuestionId { get; set; }
    public string BlobUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string? VirusScanStatus { get; set; }

    // Navigation properties
    public Submission Submission { get; set; } = null!;
    public Question? Question { get; set; }
}
