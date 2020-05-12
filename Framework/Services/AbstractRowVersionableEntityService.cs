using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Exceptions;

namespace Framework.Services.Interfaces
{
    /// <summary>
    /// Abstract class managing C, U, D operations on RowVersionable Entities 
    /// </summary>
    /// <typeparam name="TEntity">class, IIdentifiable, IRowVersionable</typeparam>
    public abstract class AbstractRowVersionableEntityService<TEntity> : AbstractIdentifiableEntityService<TEntity>, 
        IGetRowVersionableEntityService<TEntity>
        where TEntity : class, IIdentifiable, IRowVersionable
    {
        public AbstractRowVersionableEntityService(AbstractDBContext dbContext): base(dbContext) 
        { }
        
        public abstract TEntity FindRowVersion(long id, byte[] rowVersion);

        public abstract Task<TEntity> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default(CancellationToken));

        public override TEntity Update(TEntity instance)
        {
            if (FindRowVersion(instance.Id, instance.RowVersion) == null)
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");
            
            return base.Update(instance);
        }

        public override async Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await FindRowVersionAsync(instance.Id, instance.RowVersion, cancellationToken) == null)
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");
             
             return await base.UpdateAsync(instance);   
        }

        public override bool Delete(TEntity instance)
        {
            if (FindRowVersion(instance.Id, instance.RowVersion) == null)
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");

            return base.Delete(instance);
        }

        public override async Task<bool> DeleteAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await FindRowVersionAsync(instance.Id, instance.RowVersion, cancellationToken) == null)
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");

            return await base.DeleteAsync(instance, cancellationToken);       
        }
    }
}