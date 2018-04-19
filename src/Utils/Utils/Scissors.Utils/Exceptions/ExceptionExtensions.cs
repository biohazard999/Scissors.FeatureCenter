using System;
using System.Collections.Generic;

namespace Scissors.Utils.Exceptions
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Appends additional information to an <see cref="Exception"/>. Information with same key will be overwritten
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <param name="additionalInformation">The additional information.</param>
        /// <returns>The original exception. Fluent interface</returns>
        public static TException WithAdditionalInfos<TException>(
            this TException exception, 
            IEnumerable<KeyValuePair<object, object>> additionalInformation)
            where TException : Exception
        {
            Guard.AssertNotNull(exception, nameof(exception));
            Guard.AssertNotNull(additionalInformation, nameof(additionalInformation));

            foreach (var info in additionalInformation)
            {
                var key = info.Key;
                var value = info.Value;

                exception.Data[key] = value;
            }

            return exception;
        }

        /// <summary>
        /// Appends additional information to an <see cref="Exception"/>. Information with same key will be overwritten
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <param name="key">The key for the information to add to the dictionary. Must not be null</param>
        /// <param name="value">The value for the information to add to the dictionary.</param>
        /// <returns>The original exception. Fluent interface</returns>
        public static TException WithAdditionalInfo<TException>(this TException exception, object key, object value)
            where TException : Exception
        {
            Guard.AssertNotNull(exception, nameof(exception));
            Guard.AssertNotNull(key, nameof(key));

            exception.Data[key] = value;

            return exception;
        }
    }
}