using Framework.Models;
using Microsoft.AspNetCore.Http;

namespace Framework.Api
{
    public class HttpContextIdentityUser : IIdentityUser
    {
        private HttpContextAccessor _httpContextAccessor;
        public string Username => _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        public HttpContextIdentityUser(HttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;
    }
}