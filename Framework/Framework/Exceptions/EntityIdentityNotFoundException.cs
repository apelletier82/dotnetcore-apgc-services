using System;

namespace Framework.Exceptions
{
    public class EntityIdentityNotFoundException : EntityNotFoundException
    {
        public long Id { get; private set; }

        public EntityIdentityNotFoundException(long id): this(id, "")
        { }

        public EntityIdentityNotFoundException(long id, string entityClassName) 
            : this(id, entityClassName, "")
        { }

        public EntityIdentityNotFoundException(long id, string entityClassName, string message) 
            : this(id, entityClassName, message, null)
        { }

        public EntityIdentityNotFoundException(long id, string entityClassName, string message, Exception innerException) 
            : base(entityClassName, message, innerException)
            => Id = id;        
        
    }
}