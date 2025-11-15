using HD.Domain.Common;
using HD.Domain.Enums;

namespace HD.Domain.Entities;

public class User : BaseEntity, IAuditableEntity
{
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.EndUser;
    public string OktaSubjectId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // IAuditableEntity
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
}
