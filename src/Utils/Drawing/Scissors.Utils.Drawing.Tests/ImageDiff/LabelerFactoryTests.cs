using System;
using Scissors.Utils.Drawing.ImageDiff;
using Scissors.Utils.Drawing.ImageDiff.Labelers;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Drawing.Tests.ImageDiff
{
    public class LabelerFactoryTests
    {
        [Fact]
        public void FactoryThrowsWithInvalidType()
        {
            Should.Throw<ArgumentException>(() => LabelerFactory.Create((LabelerTypes)100, 0));
        }

        [Fact]
        public void FactoryCreatesBasicLabeler()
        {
            var target = LabelerFactory.Create(LabelerTypes.Basic, 0);

            target.ShouldBeOfType<BasicLabeler>();
        }

        [Fact]
        public void FactoryCreatesConnectedComponentLabeler()
        {
            var target = LabelerFactory.Create(LabelerTypes.ConnectedComponentLabeling, 0);

            target.ShouldBeOfType<ConnectedComponentLabeler>();
        }
    }
}
