using System;
using Framework.Entities;
using Framework.Models;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Framework.Api
{
    public class HttpContextTenant : ITenant
    {        
        private string _host = "";
        private long? _id = null;
        private HttpContextAccessor _httpContextAccessor;
        private ITenantEntityService _tenantEntityService;
        
        public string Host
        {
            get
            {
                if (_host == "")
                    _host = _httpContextAccessor?.HttpContext?.Request?.Host.ToUriComponent();
                return _host;   
            } 
        }

        public long Id
        {
            get 
            {                
                _id ??= _tenantEntityService?.GetTenantIdFromHost(this.Host);
                return _id != null ? (long)_id : 0;
            }
        }

        public HttpContextTenant(HttpContextAccessor httpContextAccessor, ITenantEntityService tenantEntityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantEntityService = tenantEntityService;
        }
    }
}