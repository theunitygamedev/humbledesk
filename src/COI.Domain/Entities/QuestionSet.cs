using COI.Domain.Common;
using COI.Domain.Enums;

namespace COI.Domain.Entities;

public class QuestionSet : BaseEntity, IAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public AudienceType AudienceType { get; set; }
    public int Version { get; set; }
    public QuestionSetStatus Status { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
