using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace TimerTests
{
    [TestFixture]
    public static class MeasureTests
    {
        [Test]
        public static void Measure_InvokeWithoutReturn1()
        {
            const int milliseconds = 500;

            var result = Timer.Measure.Invoke(() => Thread.Sleep(milliseconds));

            Console.WriteLine("Elapsed: " + result);

            result.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(milliseconds/10));
        }

        [Test]
        public static void Measure_InvokeWithoutReturn2()
        {
            const int milliseconds = 500;

            Timer.Measure.Invoke(
                () => Thread.Sleep(milliseconds),
                out var elapsed);

            Console.WriteLine("Elapsed: " + elapsed);

            elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(milliseconds/10));
        }

        [Test]
        public static void Measure_InvokeWithReturn()
        {
            const int milliseconds = 500;

            var result = Timer.Measure.Invoke(
                () =>
            {
                Thread.Sleep(milliseconds);
                return 123;
            },
            out var elapsed);

            Console.WriteLine("Elapsed: " + elapsed);

            result.Should().Be(123);
            elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(milliseconds/10));
        }

    }
}
