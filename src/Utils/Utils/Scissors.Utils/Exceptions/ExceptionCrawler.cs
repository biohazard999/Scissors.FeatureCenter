using System;
using System.Text;

namespace Scissors.Utils.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionCrawler
    {
        /// <summary>
        /// Extracts the error messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static string ExtractErrorMessages(Exception exception)
        {
            Guard.AssertNotNull(exception, nameof(exception));

            var sb = new StringBuilder(500);

            var ex = exception;

            while (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);

                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
