using System.Net;
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
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())   
            {                    
                using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    db.Database.EnsureDeleted();
                }
            }
        }

        [Fact]
        public void DeleteTenantDatabase()
        {
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())
            {
                using (TenantDBContext db = this.CreateTenantDBContext(loggerFactory))
                {
                    db.Database.EnsureDeleted();
                }
            }                        
        }

        [Fact]
        public async void ReadFullTestEntities()
        {
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())       
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
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())         
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

        [Fact]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void UpdateOneFullTestEntity(long id)
        {
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())  
            {       
                using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    FullTestEntityService ftes = new FullTestEntityService(db);                                                         
                    var fte = await ftes.GetAsync(id);
                    Assert.NotNull(fte);
                    fte.UpdateName(fte.Name + "-");
                    var res = await ftes.UpdateAsync(fte);
                    Assert.False(res.RowVersion.SequenceEqual(fte.RowVersion));
                }
            }
        }

        [Fact]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]        
        public async void DeleteOneFullTestEntity(long id)
        {
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())         
            {
                using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    FullTestEntityService ftes = new FullTestEntityService(db);                                                         
                    var fte = await ftes.GetAsync(id);
                    Assert.NotNull(fte);
                    var res = await ftes.DeleteAsync(fte);
                    Assert.True(res);
                }
            }
        }

        [Fact]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]        
        public async void RestoreOneDeletedFullTestEntity(long id)        
        {
            using (ILoggerFactory loggerFactory = this.CreateLoggerFactory())         
            {           
               using (DBContext db = this.CreateDbContext(loggerFactory))
                {
                    FullTestEntityService ftes = new FullTestEntityService(db);                                                         
                    var fte = await ftes.GetAsync(id);
                    Assert.NotNull(fte);
                    fte.Restore();
                    var res = ftes.Update(fte);
                    Assert.False(fte.Deleted);
                }
            }
        }
    }
}
