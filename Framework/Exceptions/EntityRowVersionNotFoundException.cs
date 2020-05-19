using System;

namespace Framework.Exceptions
{
    public class EntityRowVersionNotFoundException : EntityIdentityNotFoundException
    {
        public byte[] RowVersion { get; private set; }
        
        public EntityRowVersionNotFoundException(): this(default(byte[]), 0)
        { }

        public EntityRowVersionNotFoundException(byte[] rowVersion, long id) : this(rowVersion, id, "")
        { }

        public EntityRowVersionNotFoundException(byte[] rowVersion, long id, string entityClassName) 
            : this(rowVersion, id, entityClassName, "")
        { }

        public EntityRowVersionNotFoundException(byte[] rowVersion, long id, string entityClassName, string message) 
            : this(rowVersion, id, entityClassName, message, null)
        { }

        public EntityRowVersionNotFoundException(byte[] rowVersion, long id, string entityClassName, string message, Exception innerException) 
            : base(id, entityClassName, message, innerException)
            => RowVersion = rowVersion;        
    }
}