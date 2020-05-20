using System;

namespace Framework.Exceptions
{
    public class ApgcException : Exception
    {
        public ExceptionType ExceptionType { get; protected set; } = ExceptionType.Technical; 
        
        public ApgcException()
        { }

        public ApgcException(ExceptionType exceptionType)            
            => ExceptionType = exceptionType;

        public ApgcException(string message) 
            : base(message)
        { }

        public ApgcException(string message, ExceptionType exceptionType)
            : this(message)
            => ExceptionType = exceptionType;

        public ApgcException(string message, Exception innerException) 
            : base(message, innerException)
            => ExceptionType = (innerException is ApgcException) 
                            ? ((ApgcException)innerException).ExceptionType
                            : ExceptionType;        

        public ApgcException(string message, Exception innerException, ExceptionType exceptionType)
            :base(message, innerException)
            => ExceptionType = exceptionType;
    }
}