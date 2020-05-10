using System;
using System.Collections.Generic;
using Framework.Entities;

namespace Framework.Data.EntityTypeConfigurations
{
    public static class EntityTypeConfigurationFactory
    {
        private static Type getConstructedType(Type classType, Type interfaceType)
            => classType.MakeGenericType(interfaceType);        

        private static CustomAbstractEntityTypeConfiguration activatorCreateInstance(Type classType, Type interfaceType)
            => Activator.CreateInstance(getConstructedType(typeof(CustomIdentifiableTypeConfiguration<>), interfaceType), 
                    new object[]{ }) as CustomAbstractEntityTypeConfiguration;        

        public static CustomAbstractEntityTypeConfiguration CreateNewInstance(Type interfaceType)
        {
            if (!typeof(IEntity).IsAssignableFrom(interfaceType))
                throw new ArgumentException("interfaceType is not an IEntity");                    

            if (typeof(IIdentifiable).IsAssignableFrom(interfaceType))
                return activatorCreateInstance(typeof(CustomIdentifiableTypeConfiguration<>), interfaceType);

            if (typeof(IRowVersionable).IsAssignableFrom(interfaceType))
                return activatorCreateInstance(typeof(CustomIdentifiableTypeConfiguration<>), interfaceType);

            if (typeof(IAuditable).IsAssignableFrom(interfaceType))
                return activatorCreateInstance(typeof(CustomIdentifiableTypeConfiguration<>), interfaceType);
            
            if (typeof(ISoftDeletable).IsAssignableFrom(interfaceType))
                return activatorCreateInstance(typeof(CustomIdentifiableTypeConfiguration<>), interfaceType);

            return null;
        }

        public static IEnumerable<CustomAbstractEntityTypeConfiguration> CreateNewInstances(Type[] interfaceTypes)
        {
            IList<CustomAbstractEntityTypeConfiguration> result = new List<CustomAbstractEntityTypeConfiguration>();
            foreach(var intf in interfaceTypes)
            {
                try
                {
                    var res = CreateNewInstance(intf);
                    if (res != null)
                        result.Add(res);
                }
                catch
                {
                    continue;
                }                                
            }

            return result;
        }
    }
}