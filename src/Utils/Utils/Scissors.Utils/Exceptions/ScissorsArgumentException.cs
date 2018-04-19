using System;
using System.Runtime.Serialization;

namespace Scissors.Utils.Exceptions
{
    public class ScissorsArgumentException : ArgumentException
    {
        public ScissorsArgumentException() { }

        public ScissorsArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public ScissorsArgumentException(string message)
            : base(message) { }

        public ScissorsArgumentException(string message, Exception innerException)
            : base(message, innerException) { }

        public ScissorsArgumentException(string message, string paramName)
            : base(message, paramName) { }

        public ScissorsArgumentException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        public override string StackTrace
            => StackTraceCleaner.Clean(base.StackTrace);
    }
}