using System;
using System.Collections.Generic;
using Framework.Entities;
using Framework.Models;

namespace Framework.Data.EntityTypeConfigurations
{
    public static class EntityTypeConfigurationFactory
    {
        private static Type getConstructedType(Type classType, Type entityType)
            => classType.MakeGenericType(entityType);        

        private static CustomAbstractEntityTypeConfiguration activatorCreateInstance(Type classType, Type entityType)
            => Activator.CreateInstance(getConstructedType(classType, entityType), 
                    new object[]{ }) as CustomAbstractEntityTypeConfiguration;        

        public static CustomAbstractEntityTypeConfiguration CreateNewInstance(Type entityType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType))
                throw new ArgumentException("entityType is not assignable from IEntity");                    
                
            if (typeof(IIdentifiable).IsAssignableFrom(entityType))
                return activatorCreateInstance(typeof(CustomIdentifiableTypeConfiguration<>), entityType);

            if (typeof(IRowVersionable).IsAssignableFrom(entityType))
                return activatorCreateInstance(typeof(CustomRowVersionableEntityTypeConfiguration<>), entityType);

            if (typeof(IAuditable).IsAssignableFrom(entityType))
                return activatorCreateInstance(typeof(CustomAuditableEntityTypeConfiguration<>), entityType);
            
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType))
                return activatorCreateInstance(typeof(CustomSoftDeletableTypeConfiguration<>), entityType);

            return null;
        }

        public static CustomAbstractEntityTypeConfiguration CreateNewTenanciableIntance(ITenant tenant, Type entityType)
        {
            if(!typeof(ITenanciable).IsAssignableFrom(entityType)) 
                throw new ArgumentException("entityType is not assignable from ITenanciable");

            var constructedType = getConstructedType(typeof(CustomTenanciableEntityTypeConfiguration<>), entityType);
            return Activator.CreateInstance(constructedType, new object[]{ tenant}) as CustomAbstractEntityTypeConfiguration;
        }        
    }
}