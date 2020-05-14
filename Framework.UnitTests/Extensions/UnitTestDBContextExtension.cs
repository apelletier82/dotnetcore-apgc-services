using Framework.Models;
using Framework.UnitTests.Data.DBContexts;
using Framework.UnitTests.Models;
using Framework.UnitTests.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Framework.UnitTests.Exentions
{
    public static class UnitTestDBContextExtension
    {
        private const string SQLITE_CONNECTIONSTRING_DEFAUTL = "Data Source=UnitTestDBContext.db;";
        private const string SQLITE_CONNECTIONSTRING_TENANT = "Data Source=UnitTestTenantDBContext.db;";
        
        private static IConfigurationRoot _configuration;
        private static IConfigurationRoot Configuration
        {
            get => _configuration ??= new ConfigurationBuilder()
                                        .AddJsonFile("settings.json", true)
                                        .Build();                                
        }

        private static DbContextOptions<DBContext> CreateSQLiteDBContextOptions()
            => new DbContextOptionsBuilder<DBContext>()
                .UseSqlite(Configuration.GetConnectionString("Default") ?? SQLITE_CONNECTIONSTRING_DEFAUTL)
                .Options;

        private static DbContextOptions<TenantDBContext> CreateSQLiteTenantDBContextOptions()
            => new DbContextOptionsBuilder<TenantDBContext>()
                .UseSqlite(Configuration.GetConnectionString("Tenant") ?? SQLITE_CONNECTIONSTRING_TENANT)
                .Options;
        
        private static IIdentityUser CreateIdentityUser()
            => new TestIdentityUser(); 

        public static LoggerFactory CreateLoggerFactory(this UnitTestDBContext unitTestDBContext)
            => new LoggerFactory();

        public static TenantDBContext CreateTenantDBContext(this UnitTestDBContext unitTestDBContext, LoggerFactory loggerFactory)
        {
            var result = new TenantDBContext(CreateSQLiteTenantDBContextOptions(), CreateIdentityUser(), loggerFactory);
            result.Database.EnsureCreated();
            return result;
        }

        public static TenantEntityReadOnlyService CreateTenantEntityReadOnlyService(this UnitTestDBContext unitTestDBContext, LoggerFactory loggerFactory)
            => new TenantEntityReadOnlyService(CreateTenantDBContext(unitTestDBContext, loggerFactory));

        public static ITenant CreateLocalHostTenant(this UnitTestDBContext unitTestDBContext, LoggerFactory loggerFactory)
            => new Tenant(CreateTenantEntityReadOnlyService(unitTestDBContext, loggerFactory), "localhost");

        public static DBContext CreateDbContext(this UnitTestDBContext unitTestDBContext, LoggerFactory loggerFactory)
        {
            var result = new DBContext(CreateSQLiteDBContextOptions(), CreateLocalHostTenant(unitTestDBContext, loggerFactory), CreateIdentityUser(), loggerFactory);
            result.Database.EnsureCreated();
            return result;
        }
    }
}