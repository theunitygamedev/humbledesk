using HD.Application.Common.Interfaces;
using HD.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HD.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private Guid? _userId;
    private Guid? _tenantId;
    private bool _initialized;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public Guid? UserId
    {
        get
        {
            EnsureInitialized();
            return _userId;
        }
    }

    public Guid? TenantId
    {
        get
        {
            EnsureInitialized();
            return _tenantId;
        }
    }

    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? OktaSubjectId => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    private void EnsureInitialized()
    {
        if (_initialized)
            return;

        _initialized = true;

        if (!IsAuthenticated || string.IsNullOrEmpty(OktaSubjectId))
            return;

        // Look up user by Okta subject ID
        var user = _context.Users
            .Include(u => u.Tenant)
            .FirstOrDefault(u => u.OktaSubjectId == OktaSubjectId && u.IsActive);

        if (user != null)
        {
            _userId = user.Id;
            _tenantId = user.TenantId;
        }
    }
}
