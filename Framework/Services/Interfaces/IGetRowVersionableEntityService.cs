using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Interfaces
{
    public interface IGetRowVersionableEntityService<TEntity>
        where TEntity: class, IIdentifiable, IRowVersionable 
    {
        bool FindRowVersion(long id, byte[] rowVersion);
        bool FindRowVersion(TEntity instance);
        Task<bool> FindRowVersionAsync(long id, byte[] rowVersion, CancellationToken cancellation = default(CancellationToken));
        Task<bool> FindRowVersionAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}