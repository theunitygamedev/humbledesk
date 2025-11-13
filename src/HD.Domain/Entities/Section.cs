using HD.Domain.Common;

namespace HD.Domain.Entities;

public class Section : BaseEntity
{
    public Guid QuestionSetId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public QuestionSet QuestionSet { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
