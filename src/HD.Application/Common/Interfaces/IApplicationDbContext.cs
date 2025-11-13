using Microsoft.EntityFrameworkCore;

namespace HD.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // DbSets for HumbleDesk entities will be added here
    // Example:
    // DbSet<Ticket> Tickets { get; }
    // DbSet<Comment> Comments { get; }
    // DbSet<Attachment> Attachments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
