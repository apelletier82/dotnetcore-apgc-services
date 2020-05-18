using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Transactions;
using System.Security.AccessControl;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Framework.Entities;
using Framework.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Framework.Extensions;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Framework.Entities.Owned;

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
                .Where(e => ((e.Entity is IAuditable) &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified )))                
                .ToList()               
                .ForEach(item => 
                    {                                                
                        if (item.State == EntityState.Added)                        
                            ((IAuditable)item.Entity).Creation.DoAudit(_identityUser.Username);                                      
                        
                        // log last change everytime (on every change)                        
                        ((IAuditable)item.Entity).LastChange.DoAudit(_identityUser.Username);
                    });                                                                              

        private void applySoftDeleteToChangeTrackerEntries()
        {  
            IList<EntityEntry> softDeletedEntites = new List<EntityEntry>();

            // if entity was already "soft" deleted : do nothing: just let delete it definitively
            ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletable && ((ISoftDeletable)e.Entity).Deleted == false)
                .ToList()
                .ForEach(item => 
                {
                    item.State = EntityState.Modified; 
                    ((ISoftDeletable)item.Entity).Delete(_identityUser.Username);                                         
                    softDeletedEntites.Add(item);                                                                                                                         
                });  

            // Synchronize owned "entities" EntityState with owner's EntityState
            if (softDeletedEntites.Any())
                ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Deleted && e.Entity is Audit)
                    .ToList()
                    .ForEach(item => 
                    {
                        var owner = (softDeletedEntites
                                    .Where(e => ((ISoftDeletable)e.Entity).Deletion == item.Entity || 
                                                ((IAuditable)e.Entity).Creation == item.Entity || 
                                                ((IAuditable)e.Entity).LastChange == item.Entity))
                                    .FirstOrDefault();

                        if (owner != null)
                            item.State = owner.State;
                    });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            applySoftDeleteToChangeTrackerEntries();            
            applyAuditToChangeTrackerEntries();                    
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {               
            applySoftDeleteToChangeTrackerEntries();              
            applyAuditToChangeTrackerEntries();                      
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