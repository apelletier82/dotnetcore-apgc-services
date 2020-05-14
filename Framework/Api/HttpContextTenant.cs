using System;
using Framework.Models;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Framework.Api
{
    public class HttpContextTenant : ITenant
    {                  
        private HttpContextAccessor _httpContextAccessor;
        private ITenantEntityService _tenantEntityService;
        private string _host;
        private long? _id;
        
        public string Host { get => _host ??= _httpContextAccessor?.HttpContext?.Request?.Host.Host; }
        public long Id { get => _id ??= _tenantEntityService.GetTenantIdFromHost(Host); }

        public HttpContextTenant(HttpContextAccessor httpContextAccessor, ITenantEntityService tenantEntityService)            
        {
            _httpContextAccessor = httpContextAccessor;   
            _tenantEntityService = tenantEntityService;         
        }
    }
}