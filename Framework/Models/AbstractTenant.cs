using Framework.Services.Interfaces;

namespace Framework.Models
{
    public abstract class AbstractTenant : ITenant
    {
        private long? _id;
        protected ITenantEntityService TenantEntityService { get; private set; }
        public virtual string Host { get; protected set; }
        public virtual long Id
        {
            get
            {
                _id ??= TenantEntityService?.GetTenantIdFromHost(this.Host);
                return _id != null ? (long)_id : 0;
            }
            protected set => _id = value;
        }

        public AbstractTenant(ITenantEntityService tenantEntityService, string host)
        {
            Host = host;
            TenantEntityService = tenantEntityService;
        }
    }
}