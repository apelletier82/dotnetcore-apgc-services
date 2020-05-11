using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IGetRowVersionableEntityService<TEntity>: IGetEntityService<TEntity>
        where TEntity: class, IIdentifiable, IRowVersionable 
    {
        TEntity FindRowVersion(long id, byte[] rowVersion);
        Task<TEntity> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default(CancellationToken));
    }
}