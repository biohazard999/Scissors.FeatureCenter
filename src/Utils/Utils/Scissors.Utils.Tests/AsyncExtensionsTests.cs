using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Tests
{
    public class AsyncExtensionsTests
    {
        private async Task<T> CreateDelayedTask<T>(T value, int delay = 10)
        {
            await Task.Delay(delay);
            return value;
        }

        private async Task<bool> CreateDelayedTaskThatThrowsException(string message, int delay = 10)
        {
            await Task.Delay(delay);
            throw new Exception(message);
        }

        private async Task<T> CreateDelayedCancelableTask<T>(T value, int delay = 100, CancellationToken cancellationToken = default)
        {
            await Task.Delay(delay, cancellationToken);
            return value;
        }

        public class WaitForResult : AsyncExtensionsTests
        {

            [Fact]
            public void ShouldReturnResultWithFinishedTask()
            {
                var result = Task.FromResult(true).WaitForResult();
                result.ShouldBe(true);
            }

            [Fact]
            public void ShouldReturnValueWithDelay()
            {
                var task = CreateDelayedTask("Foo");

                var result = task.WaitForResult();

                result.ShouldBe("Foo");
            }

            [Fact]
            public void ShouldThrowDelayedException()
            {
                var task = CreateDelayedTaskThatThrowsException("Foo");

                Should.Throw<Exception>(() => task.WaitForResult())
                    .Message.ShouldBe("Foo");
            }

            [Fact]
            public void ShouldThrowTimeoutException()
            {
                var task = CreateDelayedTask("Foo", 100);

                Should.Throw<TimeoutException>(() => task.WaitForResult(10));
            }

            [Fact]
            public void ShouldThrowCancellationException()
            {
                var cts = new CancellationTokenSource();
                
                var task = CreateDelayedCancelableTask("Foo", cancellationToken: cts.Token);

                Task.Delay(50).ContinueWith(_ => cts.Cancel());

                Should.Throw<OperationCanceledException>(() => task.WaitForResult(cancellationToken: cts.Token));
            }
        }
    }
}
