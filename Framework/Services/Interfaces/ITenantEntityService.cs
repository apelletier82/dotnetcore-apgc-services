using System.Threading.Tasks;

namespace Framework.Services.Interfaces
{
    public interface ITenantEntityService
    {
         long GetTenantIdFromHost(string host);
         Task<long> GetTenantIdFromHostAsync(string host);
    }
}