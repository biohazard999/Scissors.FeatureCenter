using System;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    /// <summary>
    /// 
    /// </summary>
    public static class BitmapAnalyzerFactory
    {
        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="justNoticeableDifference">The just noticeable difference.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unrecognized Difference Detection Mode: {type}</exception>
        public static IBitmapAnalyzer Create(AnalyzerTypes type, double justNoticeableDifference)
        {
            switch (type)
            {
                case AnalyzerTypes.ExactMatch:
                    return new ExactMatchAnalyzer();
                case AnalyzerTypes.CIE76:
                    return new CIE76Analyzer(justNoticeableDifference);
                default:
                    throw new ArgumentException($"Unrecognized Difference Detection Mode: {type}");
            }
        }
    }
}