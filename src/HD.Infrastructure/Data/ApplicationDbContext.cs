using HD.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HD.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for HumbleDesk entities will be added here
    // Example:
    // public DbSet<Ticket> Tickets => Set<Ticket>();
    // public DbSet<Comment> Comments => Set<Comment>();
    // public DbSet<Attachment> Attachments => Set<Attachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the same assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
