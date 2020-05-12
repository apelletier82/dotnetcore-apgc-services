using System;
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
            => CST_ApplyConfigurationMethod;

        private static IEnumerable<MemberInfo> getDBSets(DbContext dbContext, BindingFlags bindingFlags)
            => dbContext.GetType()
                .GetProperties(bindingFlags)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType == typeof(DbSet<>))
                .Cast<MemberInfo>();

        public static IEnumerable<MemberInfo> GetAllDBSets(DbContext dbContext)
            => getDBSets(dbContext, BindingFlags.Instance | BindingFlags.NonPublic)
                .Union(getDBSets(dbContext, BindingFlags.Instance | BindingFlags.Public))
                .ToList();

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext)
            => ApplyConfigurationFromInterfacedEntites(modelBuilder, dbContext, null);

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext, ITenant tenant)
            => GetAllDBSets(dbContext)
                .Select(item => item.DeclaringType.GetGenericArguments().FirstOrDefault())
                .Where(item => item != null)
                .ToList()
                .ForEach(entityType => entityType.GetInterfaces()
                    .Where(i => typeof(IEntity).IsAssignableFrom(i))
                    .ToList()
                    .ForEach(inft => modelBuilder.GetType()
                        .GetMethods()
                        .Where(m => m.IsGenericMethod && m.Name.Equals(getApplyConfigurationMethodName(), StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault()?
                        .MakeGenericMethod(entityType.MakeGenericType())?
                        .Invoke(modelBuilder, new object[] 
                            { tenant != null ? EntityTypeConfigurationFactory.CreateNewTenanciableIntance(tenant, entityType)
                                                : EntityTypeConfigurationFactory.CreateNewInstance(entityType) })
                    )
                );
    }
}