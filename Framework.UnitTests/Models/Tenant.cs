using Framework.Models;
using Framework.Services.Interfaces;

namespace Framework.UnitTests.Models
{
    public class Tenant : ITenant
    {
        private long? _id;
        protected ITenantEntityService TenantEntityService { get; private set; }

        public virtual string Host { get; protected set; }
        public virtual long Id 
        { 
            get => _id ??= TenantEntityService.GetTenantIdFromHost(Host); 
            protected set => _id = value; 
        }

        public Tenant(ITenantEntityService tenantEntityService, string host)
        {
            Host = host;
            TenantEntityService = tenantEntityService;
        }
    }
}