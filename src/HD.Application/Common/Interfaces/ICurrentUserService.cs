namespace HD.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? TenantId { get; }
    string? UserEmail { get; }
    string? OktaSubjectId { get; }
    bool IsAuthenticated { get; }
}
