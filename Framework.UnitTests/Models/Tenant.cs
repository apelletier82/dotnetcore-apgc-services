using Framework.Models;
using Framework.Services.Interfaces;

namespace Framework.UnitTests.Models
{
    public class Tenant : AbstractTenant
    {
        public Tenant(ITenantEntityService tenantEntityService, string host) : base(tenantEntityService, host)
        { }
    }
}