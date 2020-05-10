using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Data.EntityTypeConfigurations;
using Framework.Entities;
using Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Framework.Extensions
{
    public static class ModelBuilderExtension
    {
        private const string CST_ApplyConfigurationMethod = "ApplyConfiguration";

        private static string getApplyConfigurationMethodName()
        {
            return CST_ApplyConfigurationMethod;
        }

        private static IEnumerable<MemberInfo> getDBSets(DbContext dbContext, BindingFlags bindingFlags)
        {
            return dbContext.GetType()
                .GetProperties(bindingFlags)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType == typeof(DbSet<>))
                .Cast<MemberInfo>();
        }
        public static IEnumerable<MemberInfo> GetAllDBSets(DbContext dbContext)
        {           
            List<MemberInfo> Result = new List<MemberInfo>();
            Result.AddRange(getDBSets(dbContext, BindingFlags.Instance | BindingFlags.NonPublic));
            Result.AddRange(getDBSets(dbContext, BindingFlags.Instance | BindingFlags.Public));
            return Result;
        }

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext)
            => ApplyConfigurationFromInterfacedEntites(modelBuilder, dbContext, null);        

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext, ITenant tenant)
        {
            foreach (MemberInfo miDbSet in GetAllDBSets(dbContext))
            {
                var entityType = miDbSet.DeclaringType.GetGenericArguments().FirstOrDefault();
                if (entityType == null)
                    continue;

                foreach (var intf in entityType.GetInterfaces().Where(i => typeof(IEntity).IsAssignableFrom(i)))
                {                                        
                    CustomAbstractEntityTypeConfiguration confInstance = null;
                    if (tenant != null)
                        confInstance = EntityTypeConfigurationFactory.CreateNewTenanciableIntance(tenant, entityType);
                    else
                        EntityTypeConfigurationFactory.CreateNewInstance(entityType);

                    if (confInstance == null)
                        continue;

                    var genType = entityType.MakeGenericType();
                    var appConfMtd = modelBuilder.GetType().GetMethod(getApplyConfigurationMethodName(), 1, new[] { genType })?.MakeGenericMethod(genType);
                    appConfMtd?.Invoke(modelBuilder, new object[] { confInstance });
                }
            }
        }
    }
}