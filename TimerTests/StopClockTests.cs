using System;
using System.IO;
using System.Text;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using Timer;

namespace TimerTests
{
    [TestFixture]
    public static class StopClockTests
    {
        [Test]
        public static void StopClock_WithStop()
        {
            const int milliseconds = 500;

            var stream = new MemoryStream();

            using (var sc = new StopClock(stream, "Test"))
            {
                Thread.Sleep(milliseconds);

                Console.WriteLine("Elapsed1: " + sc.Elapsed);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(milliseconds/10));

                Thread.Sleep(milliseconds);

                Console.WriteLine("Elapsed2: " + sc.Elapsed);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds * 2), TimeSpan.FromMilliseconds(milliseconds/10));

                sc.Stop();
                Thread.Sleep(milliseconds);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds * 2), TimeSpan.FromMilliseconds(milliseconds/10));
            }

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var buffer = new byte[1000];

            var bytesRead = stream.Read(buffer, 0, buffer.Length);

            bytesRead.Should().BeGreaterThan(0);
            var bufferString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine(bufferString);
            bufferString.Should().Match("Test Duration: 00:00:01*");
        }

        [Test]
        public static void StopClock_WithoutStop()
        {
            const int milliseconds = 500;

            var stream = new MemoryStream();

            using (var sc = new StopClock(stream, "Test"))
            {
                Thread.Sleep(milliseconds);

                Console.WriteLine("Elapsed1: " + sc.Elapsed);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds), TimeSpan.FromMilliseconds(milliseconds/10));

                Thread.Sleep(milliseconds);

                Console.WriteLine("Elapsed2: " + sc.Elapsed);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds * 2), TimeSpan.FromMilliseconds(milliseconds/10));

                Thread.Sleep(milliseconds);
                sc.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(milliseconds * 3), TimeSpan.FromMilliseconds(milliseconds/10));
            }

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var buffer = new byte[1000];

            var bytesRead = stream.Read(buffer, 0, buffer.Length);

            bytesRead.Should().BeGreaterThan(0);
            var bufferString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine(bufferString);
            bufferString.Should().Match("Test Duration: 00:00:01*");
        }
    }
}
