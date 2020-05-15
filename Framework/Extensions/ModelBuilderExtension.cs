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
        {
            var genApplyConfMethodType = modelBuilder.GetType().GetMethods()
                                    .Where(m => m.IsGenericMethod && 
                                                m.Name.Equals(getApplyConfigurationMethodName(), StringComparison.CurrentCultureIgnoreCase))
                                    .FirstOrDefault();
            GetAllDBSets(dbContext)
                .Select(item => item.PropertyType.GetGenericArguments().FirstOrDefault())
                .Where(item => item != null)
                .ToList()
                .ForEach(entityType => 
                {
                    foreach(var interfaceType in entityType.GetInterfaces().Where(i => typeof(IEntity).IsAssignableFrom(i)))  
                    {   
                        object inst = null;                                                                                                           
                        
                        if (tenant != null && interfaceType == typeof(ITenanciable))                                                   
                            inst = EntityTypeConfigurationFactory.CreateNewTenanciableIntance(tenant, interfaceType, entityType);                        
                        else if (tenant == null && interfaceType != typeof(ITenanciable))                        
                            inst = EntityTypeConfigurationFactory.CreateNewInstance(interfaceType, entityType);
                        
                        if (inst != null)
                            genApplyConfMethodType?.MakeGenericMethod(entityType)?.Invoke(modelBuilder, new object[] { inst });                        
                    }
                });
        }
    }
}