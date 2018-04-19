using Scissors.Utils;
using Scissors.Utils.Exceptions;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Tests
{
    public class GuardTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 2)]
        public void CanAssertArgumentIsInRange(int param, int min, int max)
        {
            Should.NotThrow(() => Guard.AssertInRange(param, nameof(param), min, max));
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(3, 1, 2)]
        public void CanAssertArgumentIsBelowRangeBound(int param, int min, int max)
        {
            Should.Throw<ScissorsArgumentOutOfRangeException>(() => Guard.AssertInRange(param, nameof(param), min, max));
        }
    }
}