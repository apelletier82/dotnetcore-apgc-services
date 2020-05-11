using System;
namespace Framework.Exceptions
{
    public class EntityNotFoundException: Exception
    { 
        public string EntityClassName {get; private set; }

        public EntityNotFoundException(string entityClassName, string message, Exception innerException): base(message, innerException)
        {
            EntityClassName = entityClassName;
        }

        public EntityNotFoundException(string entityClassName, string message): this(entityClassName, message, null)
        { }
        

        public EntityNotFoundException(string entityClassName): this(entityClassName, "", null)
        { }

        public EntityNotFoundException(): this("")
        { }
    }
}