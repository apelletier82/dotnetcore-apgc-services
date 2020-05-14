using Framework.Data;
using Framework.Models;
using Framework.UnitTests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Framework.UnitTests.Data.DBContexts
{
    public class TenantDBContext : AbstractDBContext
    {
        public TenantDBContext(DbContextOptions<TenantDBContext> options, IIdentityUser identityUser, ILoggerFactory loggerFactory) 
            : base(options, identityUser, loggerFactory)
        { }

        public DbSet<TenantEntity> Tenants { get; private set; }
    }
}