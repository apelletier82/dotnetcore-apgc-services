using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;
using Framework.Exceptions;

namespace Framework.Services.Interfaces
{
    /// <summary>
    /// <para>Asbtract class managing C, U, D operation on Row Versionable Entities.</para>
    /// <para>Check that entity id with row version exists in datastore before updating / deleting.</para>
    /// </summary>
    /// <typeparam name="TEntity">class, IIdentifiable, IRowVersionable</typeparam>
    /// <typeparam name="TDBContext">Class of AbstractDBContext</typeparam>
    public abstract class AbstractRowVersionableEntityService<TEntity, TDBContext> : AbstractEntityService<TEntity, TDBContext>, 
        IGetRowVersionableEntityService<TEntity>
        where TEntity : class, IIdentifiable, IRowVersionable
        where TDBContext: AbstractDBContext
    {
        public AbstractRowVersionableEntityService(TDBContext dbContext): base(dbContext) 
        { }

        /// <exception cref="Framework.Exceptions.EntityRowVersionNotFoundException">Entity's Id and row version not found in the DataStore</exception>
        public override TEntity Update(TEntity instance)
        {
            if (!FindRowVersion(instance))
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");
            
            return base.Update(instance);
        }

        /// <exception cref="Framework.Exceptions.EntityRowVersionNotFoundException">Entity's Id and row version not found in the DataStore</exception>
        public override async Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!(await FindRowVersionAsync(instance, cancellationToken)))
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");
             
             return await base.UpdateAsync(instance);   
        }

        /// <exception cref="Framework.Exceptions.EntityRowVersionNotFoundException">Entity's Id and row version not found in the DataStore</exception>
        public override bool Delete(TEntity instance)
        {
            if (!FindRowVersion(instance))
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");

            return base.Delete(instance);
        }

        /// <exception cref="Framework.Exceptions.EntityRowVersionNotFoundException">Entity's Id and row version not found in the DataStore</exception>
        public override async Task<bool> DeleteAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!(await FindRowVersionAsync(instance, cancellationToken)))
                throw new EntityRowVersionNotFoundException(instance.RowVersion, instance.Id, typeof(TEntity).Name, "Updated by another user");

            return await base.DeleteAsync(instance, cancellationToken);       
        }

        public abstract bool FindRowVersion(long id, byte[] rowVersion);

        public abstract bool FindRowVersion(TEntity instance);

        public abstract Task<bool> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation);        

        public abstract Task<bool> FindRowVersionAsync(TEntity instance, CancellationToken cancellationToken = default);        
    }
}