using HD.Domain.Common;
using HD.Domain.Enums;

namespace HD.Domain.Entities;

public class Tenant : BaseEntity, IAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public PlanTier PlanTier { get; set; } = PlanTier.Free;
    public bool IsActive { get; set; } = true;
    public int MaxUsers { get; set; } = 5;
    public int MaxTicketsPerMonth { get; set; } = 100;

    // IAuditableEntity
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
