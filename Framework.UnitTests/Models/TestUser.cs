using Framework.Models;
using Microsoft.Extensions.Configuration;

namespace Framework.UnitTests.Models
{
    public class TestIdentityUser : IIdentityUser
    {
        private const string USER_NAME = "xUnitUserTest";
        public string Username { get; private set; }

        public TestIdentityUser()
        {
            var conf = new ConfigurationBuilder()
                        .AddInMemoryCollection()
                        .AddInMemoryCollection()
                        .AddJsonFile("settings.json", true)
                        .Build();

            Username = conf["Username"] ?? USER_NAME;
        }                    
    }
}