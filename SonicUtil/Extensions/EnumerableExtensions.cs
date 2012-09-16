using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SonicUtil.Utility;

namespace SonicUtil.Extensions
{
    public static class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            ThrowIf.Null(items, "items");
            ThrowIf.Null(func, "func");

            return SelectAsync(items, x => func(x).ContinueWith<object>(t =>
                {
                    t.Wait();
                    return null;
                }, TaskContinuationOptions.ExecuteSynchronously));
        }

        public async static Task<IEnumerable<TResult>> SelectAsync<T, TResult>(this IEnumerable<T> items, Func<T, Task<TResult>> func)
        {
            ThrowIf.Null(items, "items");
            ThrowIf.Null(func, "func");

            var tasks = items.Select(func).ToList();
            try
            {
                return await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw new AggregateException(tasks.Where(x => x.IsFaulted).Select(x => x.Exception)).Flatten();
            }
        }
    }
}