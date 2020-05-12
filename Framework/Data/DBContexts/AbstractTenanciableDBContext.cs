using Framework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Framework.Extensions;
using System.Linq;
using Framework.Entities;

namespace Framework.Data.DBContexts
{
    public class AbstractTenanciableDBContext : AbstractDBContext
    {
        private ILogger<AbstractTenanciableDBContext> _logger;
        private ITenant _tenant;

        public AbstractTenanciableDBContext(ITenant tenant, IIdentityUser identityUser, ILoggerFactory loggerFactory) : base(identityUser, loggerFactory)            
        {
            _tenant = tenant;
            _logger = loggerFactory.CreateLogger<AbstractTenanciableDBContext>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogDebug("OnModelCreating");
            _logger.LogDebug("ApplyConfigurationFromInterfacedEntites by tenant");
            modelBuilder.ApplyConfigurationFromInterfacedEntites(this, _tenant);
            base.OnModelCreating(modelBuilder);
        }
    }
}