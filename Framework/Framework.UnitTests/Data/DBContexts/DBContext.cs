using Framework.Data.DBContexts;
using Framework.Models;
using Framework.UnitTests.Entities;
using Framework.UnitTests.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Framework.UnitTests.Data.DBContexts
{
    public class DBContext : AbstractTenanciableDBContext
    {
        public DBContext(DbContextOptions<DBContext> options, ITenant tenant, IIdentityUser identityUser, ILoggerFactory loggerFactory) 
            : base(options, tenant, identityUser, loggerFactory)
        { }

        public DbSet<FullTestEntity> FullTestEntities { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.ManageSqliteRowVersionConvertion(modelBuilder);            
        }        
        
    }
}