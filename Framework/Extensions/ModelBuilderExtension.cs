using System;
using System.Linq;
using Framework.Data.EntityTypeConfigurations;
using Framework.Entities;
using Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Framework.Extensions
{
    public static class ModelBuilderExtension
    {
        private static string applyConfigurationMethodName { get; set; } = "ApplyConfiguration";

        private static string getApplyConfigurationMethodName()
            => applyConfigurationMethodName;

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext)
            => ApplyConfigurationFromInterfacedEntites(modelBuilder, dbContext, null);

        public static void ApplyConfigurationFromInterfacedEntites(this ModelBuilder modelBuilder, DbContext dbContext, ITenant tenant)
        {
            var genApplyConfMethodType = modelBuilder.GetType().GetMethods()
                                    .Where(m => m.IsGenericMethod &&
                                                m.Name.Equals(getApplyConfigurationMethodName(), StringComparison.CurrentCultureIgnoreCase))
                                    .FirstOrDefault();

            modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType != null)
                .Select(e => e.ClrType)
                .ToList()
                .ForEach(entityType =>
                {
                    var genericMethod = genApplyConfMethodType?.MakeGenericMethod(entityType);
                    if (tenant == null)
                        foreach (var interfaceType in entityType.GetInterfaces().Where(i => typeof(IEntity).IsAssignableFrom(i) && i != typeof(ITenanciable)))
                        {
                            var inst = EntityTypeConfigurationFactory.CreateNewInstance(interfaceType, entityType);
                            if (inst != null)
                                genericMethod?.Invoke(modelBuilder, new object[] { inst });
                        }
                    else
                        foreach (var interfaceType in entityType.GetInterfaces().Where(i => typeof(IEntity).IsAssignableFrom(i) && i == typeof(ITenanciable)))
                            genericMethod?.Invoke(modelBuilder, new object[] { EntityTypeConfigurationFactory.CreateNewTenanciableIntance(tenant, interfaceType, entityType) });
                });
        }
    }
}