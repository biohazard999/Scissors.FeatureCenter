using System;
using System.Linq;

namespace Scissors.Utils.Exceptions
{
    internal static class StackTraceCleaner
    {
        public static string Clean(string baseStackTrace)
        {
            var stacktrace = baseStackTrace
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !line.StartsWith("   at " + typeof(Guard).FullName, StringComparison.Ordinal));
            
            return string.Join(Environment.NewLine, stacktrace);
        }
    }
}