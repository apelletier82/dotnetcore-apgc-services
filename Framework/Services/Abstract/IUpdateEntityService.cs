using System.Threading;
using System.Threading.Tasks;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IUpdateEntityService<TEntity> 
        where TEntity: class, IEntity
    {
        TEntity Update(TEntity instance);         
        Task<TEntity> UpdateAsync(TEntity instance, CancellationToken cancellationToken = default(CancellationToken));
    }
}