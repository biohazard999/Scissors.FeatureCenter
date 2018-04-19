using System;
using Scissors.Utils.Drawing.ImageDiff;
using Scissors.Utils.Drawing.ImageDiff.BoundingBoxes;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Drawing.Tests.ImageDiff
{
    public class BoundingBoxFactoryTests
    {
        [Fact]
        public void FactoryThrowsWithInvalidType()
        {
            Should.Throw<ArgumentException>(() => BoundingBoxIdentifierFactory.Create((BoundingBoxModes)100, 0));
        }

        [Fact]
        public void FactoryCreatesSingleBoundingBoxIdentifier()
        {
            var target = BoundingBoxIdentifierFactory.Create(BoundingBoxModes.Single, 0);
            target.ShouldBeOfType<SingleBoundingBoxIdentifier>();
        }

        [Fact]
        public void FactoryCreatesMultipleBoundingBoxIdentifier()
        {
            var target = BoundingBoxIdentifierFactory.Create(BoundingBoxModes.Multiple, 0);
            target.ShouldBeOfType<MultipleBoundingBoxIdentifier>();
        }
    }
}
