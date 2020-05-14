using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Framework.Services.Interfaces;
using Framework.UnitTests.Data.DBContexts;

namespace Framework.UnitTests.Services
{
    public class TenantEntityReadOnlyService: ITenantEntityService
    {
        private TenantDBContext _testDBContext;
        public TenantEntityReadOnlyService(TenantDBContext testDBContext)
            => _testDBContext = testDBContext;

        public long GetTenantIdFromHost(string host)
            =>_testDBContext.Tenants
                .Where(t => t.Host.ToLower() == host.ToLower())
                .Select(s => s.Id)
                .FirstOrDefault();                  

        public async Task<long> GetTenantIdFromHostAsync(string host, CancellationToken cancellationToken = default(CancellationToken)) 
            => await Task.Run<long>(() => GetTenantIdFromHost(host), cancellationToken);       
    }
}