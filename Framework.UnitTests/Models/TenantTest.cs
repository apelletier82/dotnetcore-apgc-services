using Framework.Models;

namespace Framework.UnitTests.Models
{
    public class TenantTest: ITenant
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Host { get; private set; }

        public TenantTest(string name, string host)
        {
            Name = name;
            Host = host;
        }
    }
}