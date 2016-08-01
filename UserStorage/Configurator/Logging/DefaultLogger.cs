using System;
using System.Diagnostics;
using System.Threading;
using Storage.Interfaces.Logger;

namespace Configurator.Logging
{
    [Serializable]
    public class DefaultLogger : ILogger
    {
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;
        private readonly Mutex mutex = new Mutex(false, "loggingMutex");

        public DefaultLogger()
        {
            traceSwitch = new BooleanSwitch("traceSwitch", string.Empty);
            traceSource = new TraceSource("trace");
        }

        public void Log(TraceEventType type, string message)
        {
            if (traceSwitch.Enabled)
            {
                mutex.WaitOne();
                traceSource.TraceEvent(type, 0, message);
                mutex.ReleaseMutex();
            }
        }
    }
}