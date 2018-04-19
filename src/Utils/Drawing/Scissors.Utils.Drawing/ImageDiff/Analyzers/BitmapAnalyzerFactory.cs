using System;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    public static class BitmapAnalyzerFactory
    {
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