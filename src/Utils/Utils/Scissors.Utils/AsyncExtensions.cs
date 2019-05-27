using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scissors.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static T WaitForResult<T>(this Task<T> task, int? delay = null, CancellationToken cancellationToken = default)
        {
            var result = default(T);
            Exception exception = null;

            var @event = new ManualResetEvent(false);

            task.ContinueWith(t =>
            {
                if(t.IsCanceled)
                {
                    exception = new OperationCanceledException();
                    @event.Set();
                    return;
                }
                if(cancellationToken.IsCancellationRequested)
                {
                    exception = new OperationCanceledException();
                    @event.Set();
                    return;
                }
                if(t.IsFaulted)
                {
                    exception = t.Exception.InnerException;
                    @event.Set();
                    return;
                }
                if(t.IsCompleted)
                {
                    result = t.Result;
                    @event.Set();
                    return;
                }
            });
            
            if(delay.HasValue)
            {
                if(!@event.WaitOne(delay.Value, false))
                {
                    throw new TimeoutException();
                }
            }
            else
            {
                @event.WaitOne();
            }

            if(exception != null)
            {
                throw exception;
            }

            return result;
        }
    }
}
