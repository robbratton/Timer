using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Timer
{
    /// <summary>
    /// Time operations
    /// </summary>
    /// <example>
    /// using (new StopClock(Console.OpenStandardError, "Optional Title")){
    ///     (Do stuff)
    /// }
    /// at the close of the using, the elapsed time will be written to the stream.
    /// </example>.
    public class StopClock : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _title;
        private readonly Stream _stream;
        private readonly StreamWriter _streamWriter;
        private bool _disposed;
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">Stream for output.</param>
        /// <param name="title">Title</param>
        /// <remarks>You can use Console.OpenStandardOutput or Console.OpenStandardError, etc.</remarks>
        public StopClock(Stream stream, string title = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException(nameof(stream) + " must be write-able.");
            }

            _stream = stream;
            _streamWriter = new StreamWriter(stream);
            _title = title;
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Stop
        /// </summary>
        /// <returns></returns>
        public Stopwatch Stop()
        {
            _stopwatch.Stop();

            return _stopwatch;
        }

        /// <summary>
        /// Gets the current elapsed time.
        /// </summary>
        /// <remarks>Does not stop the stopwatch.</remarks>
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        /// <summary>
        /// Print the current elapsed time.
        /// </summary>
        public void Print()
        {
            if (_streamWriter != null)
            {
                var title = "";
                if (_title != null)
                {
                    title = _title + " ";
                }

                _streamWriter.WriteLine($"{title}Duration: {_stopwatch.Elapsed}");
                _streamWriter.Flush();
                _stream.Flush();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Overridable Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; 
      
            if (disposing) {
                _handle.Dispose();

                Stop();
                Print();
            }
      
            _disposed = true;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~StopClock()
        {
            Dispose(false);
        }
    }
}
