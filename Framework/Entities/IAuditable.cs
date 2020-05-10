using System;
using Framework.Entities.Owned;

namespace Framework.Entities
{
    public interface IAuditable: IEntity
    {
        Audit Creation { get; }
        Audit LastChange { get; }        
    }
}