using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Framework.Entities;
using Framework.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Framework.Extensions;

namespace Framework.Data
{
    public abstract class CustomDBContext : DbContext
    {    
        ISimpleUser _simpleUser;
        ILogger<CustomDBContext> _logger;       
        
        public CustomDBContext(ISimpleUser simpleUser, ILoggerFactory loggerFactory)
        {
            _simpleUser = simpleUser;
            _logger = loggerFactory.CreateLogger<CustomDBContext>();
        }

        private void applyAuditToChangeTrackerEntries()
        {
            foreach(var entry in ChangeTracker
                                .Entries()
                                .Where(e => ((e is IAuditable) &&
                                            (e.State == EntityState.Added || e.State == EntityState.Modified)))
                    )
            {
                IAuditable ent = (IAuditable)entry.Entity;
                if (entry.State == EntityState.Added)
                    ent.Creation.DoAudit(_simpleUser.Username);
                else if (entry.State == EntityState.Modified)
                    ent.LastChange.DoAudit(_simpleUser.Username);
            }                                                
        }

        private void applyDeletionToChangeTrackerEntries()
        {
            foreach(var entry in ChangeTracker.Entries().Where(e => (e is ISoftDeletable) && (e.State == EntityState.Deleted)))
            {
                ISoftDeletable ent = (ISoftDeletable)entry.Entity;
                ent.Delete(_simpleUser.Username);
                entry.State = EntityState.Modified;                
            }
        }

        private void applyModificationToChangeTrackerEntities()
        {
            applyDeletionToChangeTrackerEntries();
            applyAuditToChangeTrackerEntries();                
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            applyModificationToChangeTrackerEntities();        
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {   
            applyModificationToChangeTrackerEntities();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationFromInterfacedEntites(this);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}