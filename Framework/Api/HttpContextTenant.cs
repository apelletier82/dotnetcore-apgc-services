using Framework.Models;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Framework.Api
{
    public class HttpContextTenant : AbstractTenant
    {                  
        private HttpContextAccessor _httpContextAccessor;        
        public override string Host
        {
            get
            {
                if (base.Host == "")
                    base.Host = _httpContextAccessor?.HttpContext?.Request?.Host.ToUriComponent();
                return base.Host;   
            } 
        }

        public HttpContextTenant(HttpContextAccessor httpContextAccessor, ITenantEntityService tenantEntityService)
            :base(tenantEntityService, "")
        {
            _httpContextAccessor = httpContextAccessor;            
        }
    }
}