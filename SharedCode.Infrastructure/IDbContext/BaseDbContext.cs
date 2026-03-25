using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedCode.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Infrastructure.IDbContext;

public abstract class BaseDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
   

    protected BaseDbContext(
        DbContextOptions options,
         IHttpContextAccessor httpContextAccessor
    ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAudit();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAudit()
    {
        var userName = GetUserName();

        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.created = DateTime.UtcNow;
                entry.Entity.updated = DateTime.UtcNow;
                entry.Entity.created_by = userName;
                entry.Entity.updated_by = userName;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.updated = DateTime.UtcNow;
                entry.Entity.updated_by = userName;
            }
        }
    }
    private string GetUserName()
    {
        return _httpContextAccessor
            ?.HttpContext
            ?.User
            ?.Identity
            ?.Name
            ?? "anonymous";
    }
}