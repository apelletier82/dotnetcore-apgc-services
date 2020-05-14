using System.Linq;
using Framework.UnitTests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.UnitTests.Extensions
{
    public static class DbContextSqliteExtension
    {
        public static void ManageSqliteRowVersionConvertion(this DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsSqlite())
            {
                var timestampProperties = modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(byte[])                        
                        && p.ValueGenerated == ValueGenerated.OnAddOrUpdate
                        && p.IsConcurrencyToken);

                foreach (var property in timestampProperties)
                {
                    property.SetValueConverter(new SqliteTimestampConverter()); 
                    property.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }            
        }
    }
}