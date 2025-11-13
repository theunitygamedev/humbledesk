using HD.Domain.Common;

namespace HD.Domain.Entities;

public class Submission : BaseEntity
{
    public Guid AssignmentId { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string SubmittedBy { get; set; } = string.Empty;
    public string? AttestationText { get; set; }
    public string? Signature { get; set; }
    public string? IpAddress { get; set; }
    public string? SnapshotJson { get; set; }

    // Navigation properties
    public Assignment Assignment { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
