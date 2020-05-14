using System.Xml.Schema;
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

        private static IEnumerable<PropertyInfo> getDBSets(DbContext dbContext, BindingFlags bindingFlags)
            => dbContext.GetType()
                .GetProperties(bindingFlags)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));                

        public static IEnumerable<PropertyInfo> GetAllDBSets(DbContext dbContext)
            => getDBSets(dbContext, BindingFlags.Instance | BindingFlags.NonPublic)
                .Union(getDBSets(dbContext, BindingFlags.Instance | BindingFlags.Public));

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext)
            => ApplyConfigurationFromInterfacedEntites(modelBuilder, dbContext, null);
        
        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext, ITenant tenant)
            => GetAllDBSets(dbContext)
                .Select(item => item.PropertyType.GetGenericArguments().FirstOrDefault())
                .Where(item => item != null)
                .ToList()
                .ForEach(entityType => 
                { 
                    var genApplyConfMethodType = modelBuilder.GetType().GetMethods()
                                    .Where(m => m.IsGenericMethod && 
                                        m.Name.Equals(getApplyConfigurationMethodName(), StringComparison.CurrentCultureIgnoreCase))
                                    .FirstOrDefault();  

                    foreach(var interfaceType in entityType.GetInterfaces().Where(i => typeof(IEntity).IsAssignableFrom(i)))                                                                                                                
                        if (tenant != null) 
                        {
                            if (interfaceType == typeof(ITenanciable))
                            {
                                var inst = EntityTypeConfigurationFactory.CreateNewTenanciableIntance(tenant, interfaceType, entityType);
                                if (inst != null)
                                    genApplyConfMethodType.MakeGenericMethod(entityType)?.Invoke(modelBuilder, new object[] { inst });
                            }
                        }
                        else if (interfaceType != typeof(ITenanciable))
                        {
                            var inst = EntityTypeConfigurationFactory.CreateNewInstance(interfaceType, entityType);
                            if (inst != null)
                                genApplyConfMethodType.MakeGenericMethod(entityType)?.Invoke(modelBuilder, new object[] { inst });
                        }
                });
    }
}