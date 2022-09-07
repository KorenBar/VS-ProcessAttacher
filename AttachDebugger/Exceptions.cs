using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachDebugger
{
    public enum ErrorCode
    {
        None = 0,
        General = 1,
        MissingArgument = 1 << 1,
        NoDebugger = 1 << 2,
        TransportNotFound = 1 << 3,
        EngineNotFound = 1 << 4,
        ProcessNotFound = 1 << 5,
        AttachFailure = 1 << 6
    }

    public class DebugException : Exception
    {
        private ErrorCode _ErrorCode;
        /// <summary>
        /// Get the most first error code
        /// </summary>
        public ErrorCode ErrorCode {
            get
            {
                Exception innerDebugEx = this;

                do // get the next inner DebugException
                    innerDebugEx = innerDebugEx.InnerException;
                while (innerDebugEx != null && !(innerDebugEx is DebugException));

                return (innerDebugEx as DebugException)?.ErrorCode ?? _ErrorCode;
            }
        }

        public DebugException() : this(string.Empty) { }
        public DebugException(string message) : this(ErrorCode.None, message) { }
        public DebugException(string message, Exception innerException) : this(ErrorCode.None, message, innerException) { }
        public DebugException(ErrorCode errorCode, string message) : this(errorCode, message, null) { }
        public DebugException(ErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            _ErrorCode = errorCode;
        }
    }
}
