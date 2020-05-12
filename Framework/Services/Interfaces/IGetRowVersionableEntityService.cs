using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Interfaces
{
    public interface IGetRowVersionableEntityService<TEntity>
        where TEntity: class, IIdentifiable, IRowVersionable 
    {
        TEntity FindRowVersion(long id, byte[] rowVersion);
        Task<TEntity> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default(CancellationToken));
    }
}