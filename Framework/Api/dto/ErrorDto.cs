using System;

namespace Framework.Api.dto
{
    public class ErrorDto
    {
        public String Title { get; set; }
        public String Message { get; set; }
        public String TraceId { get; set; }
        public int? ErrorCode { get; set; }
        public String ErrorType { get; set; }        

        public ErrorDto()
        { }

        public ErrorDto(String message)
            : this(null, message, null, null, null)
        { }

        public ErrorDto(String title, String message)
            : this(title, message, null, null, null)
        { }

        public ErrorDto(String title, String message, String traceId)
            : this(title, message, traceId, null, null)
        { }

        public ErrorDto(String title, String message, String traceId, String errorType)
            : this(title, message, traceId, null, errorType)
        { }

        public ErrorDto(String title, String message, String traceId, int? errorCode)
            : this(title, message, traceId, errorCode, null)
        { }

        public ErrorDto(String title, String message, String traceId, int? errorCode, String errorType)
        {
            Title = title;
            Message = message;
            TraceId = traceId;
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
}