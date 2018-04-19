using System;
using Scissors.Utils.Drawing.ImageDiff;
using Scissors.Utils.Drawing.ImageDiff.Analyzers;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Drawing.Tests.ImageDiff
{
    public class AnalyzerFactoryTests
    {
        [Fact]
        public void FactoryThrowsWithInvalidType()
        {
            Should.Throw<ArgumentException>(() => BitmapAnalyzerFactory.Create((AnalyzerTypes)100, 2.3));
        }

        [Fact]
        public void FactoryCreatesExactMatchAnalyzer()
        {
            var target = BitmapAnalyzerFactory.Create(AnalyzerTypes.ExactMatch, 2.3);
            target.ShouldBeOfType<ExactMatchAnalyzer>();
        }

        [Fact]
        public void FactoryCreatesCIE76Analyzer()
        {
            var target = BitmapAnalyzerFactory.Create(AnalyzerTypes.CIE76, 2.3);

            target.ShouldBeOfType<CIE76Analyzer>();
        }
    }
}
