using System;
using System.Diagnostics;

namespace Timer
{
    public static class Measure
    {
        /// <summary>
        /// Invoke an action with a return value and time it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static T Invoke<T>(Func<T> action, out TimeSpan timeSpan)
        {
            var stopwatch = Stopwatch.StartNew();

            var res = action.Invoke();

            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;

            return res;
        }

        /// <summary>
        /// Invoke an action without a return value and time it.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="timeSpan"></param>
        public static void Invoke(Action action, out TimeSpan timeSpan)
        {
            var stopwatch = Stopwatch.StartNew();

            action.Invoke();

            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;
        }

        /// <summary>
        /// Invoke an action and time it returning the execution time.
        /// </summary>
        /// <param name="action"></param>
        public static TimeSpan Invoke(Action action)
        {
            var stopwatch = Stopwatch.StartNew();

            action.Invoke();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

    }
}
