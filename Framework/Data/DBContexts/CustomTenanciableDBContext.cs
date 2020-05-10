using System.Linq;
using Framework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Framework.Extensions;

namespace Framework.Data.DBContexts
{
    public class CustomTenanciableDBContext : CustomDBContext
    {
        private ILogger<CustomTenanciableDBContext> _logger;
        private ITenant _tenant;

        public CustomTenanciableDBContext(ITenant tenant, ISimpleUser simpleUser, ILoggerFactory loggerFactory) : base(simpleUser, loggerFactory)            
        {
            _tenant = tenant;
            _logger = loggerFactory.CreateLogger<CustomTenanciableDBContext>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationFromInterfacedEntites(this, _tenant);
            base.OnModelCreating(modelBuilder);
        }
    }
}