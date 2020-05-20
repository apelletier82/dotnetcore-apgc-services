using Framework.Data;
using Framework.Models;
using Framework.UnitTests.Entities;
using Framework.UnitTests.Extensions;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.ManageSqliteRowVersionConvertion(modelBuilder);                        
        }
    }
}