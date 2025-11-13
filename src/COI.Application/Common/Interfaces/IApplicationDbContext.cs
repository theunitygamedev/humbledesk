using COI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace COI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<QuestionSet> QuestionSets { get; }
    DbSet<Section> Sections { get; }
    DbSet<Question> Questions { get; }
    DbSet<Option> Options { get; }
    DbSet<BranchRule> BranchRules { get; }
    DbSet<Assignment> Assignments { get; }
    DbSet<Submission> Submissions { get; }
    DbSet<Answer> Answers { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Comment> Comments { get; }
    DbSet<Attachment> Attachments { get; }
    DbSet<AuditEvent> AuditEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
