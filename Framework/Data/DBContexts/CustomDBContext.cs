using System.Collections.Immutable;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Framework.Entities;
using Framework.Data.EntityTypeConfigurations;

namespace Framework.Data
{
    public abstract class CustomDBContext: DbContext
    {
        private IEnumerable<MemberInfo> getDBSets(BindingFlags bindingFlags)
        {                                   
            return GetType()
                .GetProperties(bindingFlags)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType == typeof(DbSet<>))
                .Cast<MemberInfo>();
        }
        private IEnumerable<MemberInfo> getAllDBSets()
        {
            List<MemberInfo> Result = new List<MemberInfo>(); 
            Result.AddRange(getDBSets(BindingFlags.Instance | BindingFlags.NonPublic));
            Result.AddRange(getDBSets(BindingFlags.Instance | BindingFlags.Public));
            return Result;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1st : Get all entites 
            foreach(MemberInfo miDbSet in getAllDBSets())
            {                
                var entityType = miDbSet.DeclaringType.GenericTypeArguments.FirstOrDefault();
                if (entityType == null)
                    continue;

                foreach(var intf in entityType.GetInterfaces())
                {
                    if (!typeof(IEntity).IsAssignableFrom(intf))
                        continue;
                                        
                    var confInstance = EntityTypeConfigurationFactory.CreateNewInstance(intf);
                    var genType = entityType.MakeGenericType();                    
                    var appConfMtd = modelBuilder.GetType().GetMethod("ApplyConfiguration", 1, new[] { genType })?
                                            .MakeGenericMethod(genType);
                    appConfMtd.Invoke(modelBuilder, new object[] { confInstance });
                }                
            }

            // 2nd : Assembly's configurations 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}