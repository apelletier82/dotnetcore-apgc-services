using System.Threading;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Framework.Entities;

namespace Framework.Services.Abstract
{
    public interface IGetEntityService<TEntity> 
        where TEntity: class, IEntity        
    {
         IEnumerable<TEntity> GetAll();         
         Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellation = default(CancellationToken));         
    }
}