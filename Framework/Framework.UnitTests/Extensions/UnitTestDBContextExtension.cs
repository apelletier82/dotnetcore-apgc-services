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
        private static string sqLiteConnectionStringDefault { get; set; } = "Data Source=UnitTestDBContext.db;";
        private static string sqLiteConnectionStringTenant { get; set; } = "Data Source=UnitTestTenantDBContext.db;";

        private static IConfigurationRoot _configuration;
        private static IConfigurationRoot Configuration
        {
            get => _configuration ??= new ConfigurationBuilder()
                                        .AddJsonFile("settings.json", true)
                                        .Build();
        }

        private static DbContextOptions<DBContext> CreateSQLiteDBContextOptions(ILoggerFactory loggerFactory)
            => new DbContextOptionsBuilder<DBContext>()
                .UseSqlite(Configuration.GetConnectionString("Default") ?? sqLiteConnectionStringDefault)
                .UseLoggerFactory(loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

        private static DbContextOptions<TenantDBContext> CreateSQLiteTenantDBContextOptions(ILoggerFactory loggerFactory)
            => new DbContextOptionsBuilder<TenantDBContext>()
                .UseSqlite(Configuration.GetConnectionString("Tenant") ?? sqLiteConnectionStringTenant)
                .UseLoggerFactory(loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

        private static IIdentityUser CreateIdentityUser()
            => new TestIdentityUser();

        public static ILoggerFactory CreateLoggerFactory(this UnitTestDBContext unitTestDBContext)
            => LoggerFactory.Create(builder => builder.AddConsole());

        public static TenantDBContext CreateTenantDBContext(this UnitTestDBContext unitTestDBContext, ILoggerFactory loggerFactory)
        {
            var result = new TenantDBContext(CreateSQLiteTenantDBContextOptions(loggerFactory), CreateIdentityUser(), loggerFactory);
            result.Database.EnsureCreated();
            return result;
        }

        public static TenantEntityReadOnlyService CreateTenantEntityReadOnlyService(this UnitTestDBContext unitTestDBContext, ILoggerFactory loggerFactory)
            => new TenantEntityReadOnlyService(CreateTenantDBContext(unitTestDBContext, loggerFactory));

        public static ITenant CreateLocalHostTenant(this UnitTestDBContext unitTestDBContext, ILoggerFactory loggerFactory)
            => new Tenant(CreateTenantEntityReadOnlyService(unitTestDBContext, loggerFactory), "localhost");

        public static DBContext CreateDbContext(this UnitTestDBContext unitTestDBContext, ILoggerFactory loggerFactory)
        {
            var result = new DBContext(CreateSQLiteDBContextOptions(loggerFactory), CreateLocalHostTenant(unitTestDBContext, loggerFactory), CreateIdentityUser(), loggerFactory);
            result.Database.EnsureCreated();
            return result;
        }
    }
}