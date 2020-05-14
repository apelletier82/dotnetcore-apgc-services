using Framework.Models;

namespace Framework.UnitTests.Entities
{
    public class TenantEntity : ITenant
    {
        public string Host { get; private set; }
        public long Id { get; private set; }

        public TenantEntity(string host, long id)
        {
            Host = host;
            Id = id;
        }
    }
}