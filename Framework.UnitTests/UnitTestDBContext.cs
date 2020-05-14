using System;
using System.Linq;
using Framework.UnitTests.Data.DBContexts;
using Framework.UnitTests.Entities;
using Framework.UnitTests.Exentions;
using Framework.UnitTests.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Framework.UnitTests
{
    public class UnitTestDBContext
    {             

        [Fact]
        public void DeleteDatabase()
        {
            using (LoggerFactory loggerFactory = this.CreateLoggerFactory())                       
                using (DBContext db = this.CreateDbContext(loggerFactory))
                    db.Database.EnsureDeleted();
        }

        [Fact]
        public void DeleteTenantDatabase()
        {
            using (LoggerFactory loggerFactory = this.CreateLoggerFactory())                       
                using (TenantDBContext db = this.CreateTenantDBContext(loggerFactory))
                    db.Database.EnsureDeleted();
        }

        [Fact]
        public async void ReadFullTestEntities()
        {
            using (LoggerFactory loggerFactory = this.CreateLoggerFactory())
            {           
                using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    FullTestEntityService ftes = new FullTestEntityService(db);
                    var lst = await ftes.GetListAsync();                
                    Assert.NotNull(lst);
                }
            }
        }

        [Fact]
        public async void AddFullTestEntity()
        {
            using (LoggerFactory loggerFactory = this.CreateLoggerFactory())
            {           
                using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    FullTestEntityService ftes =  new FullTestEntityService(db);                    
                    var tenant = this.CreateLocalHostTenant(loggerFactory);
                    var fte = new FullTestEntity(tenant, string.Format("Test name {0}", (await ftes.GetListAsync())?.Count() + 1), DateTime.Now);
                    
                    Assert.Equal(tenant.Id, fte.TenantID);
                    Assert.Equal(fte.Date, DateTime.Now.Date);

                    var res = await ftes.AddAsync(fte);
                    Assert.NotNull(res);
                }
            }            
        }
    }
}
