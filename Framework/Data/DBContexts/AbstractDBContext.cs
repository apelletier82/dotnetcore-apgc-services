using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Framework.Entities;
using Framework.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Framework.Extensions;
using Framework.Data.EntityTypeConfigurations;
using Framework.Entities.Owned;
using System.Threading;

namespace Framework.Data
{
    public abstract class AbstractDBContext : DbContext
    {    
        private IIdentityUser _identityUser;
        private ILogger<AbstractDBContext> _logger;       
        
        public AbstractDBContext(DbContextOptions options, IIdentityUser identityUser, ILoggerFactory loggerFactory)
            :base(options)
        {
            _identityUser = identityUser;
            _logger = loggerFactory.CreateLogger<AbstractDBContext>();
        }

        private void applyAuditToChangeTrackerEntries()
            => ChangeTracker.Entries()
                .Where(e => ((e is IAuditable) &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified)))
                .ToList()
                .ForEach(entry => 
                    {
                        var ent = (IAuditable)entry.Entity;
                        if (entry.State == EntityState.Added)                        
                            ent.Creation.DoAudit(_identityUser.Username);                                       
                        // log last change everytime 
                        ent.LastChange.DoAudit(_identityUser.Username);
                    });                                                        

        private void applyDeletionToChangeTrackerEntries()
            => ChangeTracker.Entries()
                .Where(e => (e is ISoftDeletable) && (e.State == EntityState.Deleted))
                .ToList()
                .ForEach(item => 
                    {                        
                        ((ISoftDeletable)item.Entity).Delete(_identityUser.Username);
                        item.State = EntityState.Modified; 
                    });                         

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

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {   
            applyModificationToChangeTrackerEntities();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogDebug("OnModelCreating");
            
            _logger.LogDebug("ApplyConfigurationFromInterfacedEntites");
            modelBuilder.ApplyConfigurationFromInterfacedEntites(this);

            _logger.LogDebug("ApplyConfigurationsFromAssembly");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}