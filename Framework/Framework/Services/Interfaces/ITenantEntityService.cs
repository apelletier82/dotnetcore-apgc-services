using System.Threading;
using System.Threading.Tasks;

namespace Framework.Services.Interfaces
{
    public interface ITenantEntityService
    {
         long GetTenantIdFromHost(string host);
         Task<long> GetTenantIdFromHostAsync(string host, CancellationToken cancellation = default(CancellationToken));
    }
}