using System;
using Framework.Entities;
using Framework.Models;

namespace Framework.Data.EntityTypeConfigurations
{
    public static class EntityTypeConfigurationFactory
    {
        private static Type getGenericClassType(Type classType, Type entityType)
            => classType.MakeGenericType(entityType);

        private static object activatorCreateInstance(Type classType, Type entityType)
            => Activator.CreateInstance(getGenericClassType(classType, entityType), new object[] { });

        public static Type GetGenericEntityTypeConfigurationType(Type interfaceType)
        {
            if (!typeof(IEntity).IsAssignableFrom(interfaceType))
                throw new ArgumentException("interfaceType is not assignable from IEntity");

            if (interfaceType == typeof(IIdentifiable))
                return typeof(CustomIdentifiableTypeConfiguration<>);

            if (interfaceType == typeof(IRowVersionable))
                return typeof(CustomRowVersionableEntityTypeConfiguration<>);

            if (interfaceType == typeof(IAuditable))
                return typeof(CustomAuditableEntityTypeConfiguration<>);

            if (interfaceType == typeof(ISoftDeletable))
                return typeof(CustomSoftDeletableTypeConfiguration<>);

            return null;
        }

        public static object CreateNewInstance(Type interfaceType, Type entityType)
        {
            var genEntityTypeConfiguration = GetGenericEntityTypeConfigurationType(interfaceType);
            if (genEntityTypeConfiguration == null)
                return null;
            return activatorCreateInstance(genEntityTypeConfiguration, entityType);
        }

        public static object CreateNewTenanciableIntance(ITenant tenant, Type interfaceType, Type entityType)
        {
            if (typeof(ITenanciable) != interfaceType)
                throw new ArgumentException("interfaceType is not ITenanciable");

            return Activator.CreateInstance(getGenericClassType(typeof(CustomTenanciableEntityTypeConfiguration<>), entityType),
                new object[] { tenant });
        }
    }
}