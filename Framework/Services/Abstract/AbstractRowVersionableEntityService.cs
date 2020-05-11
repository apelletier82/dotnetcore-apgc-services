using System.Threading;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public abstract class AbstractRowVersionableEntityService<TEntity> : AbstractIdentifiableEntityService<TEntity>, IGetRowVersionableEntityService<TEntity>
        where TEntity : class, IIdentifiable, IRowVersionable
    {
        public AbstractRowVersionableEntityService(CustomDBContext dbContext): base(dbContext) {}
        
        public abstract TEntity FindRowVersion(long id, byte[] rowVersion);

        public abstract Task<TEntity> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default);
    }
}