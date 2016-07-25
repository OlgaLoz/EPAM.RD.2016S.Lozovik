using System;
using System.Diagnostics;

namespace Storage.Logging
{
    public class Logger
    {
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return instance.Value; } }

        private Logger()
        {
            traceSwitch = new BooleanSwitch("traceSwitch", "");
            traceSource = new TraceSource("trace");
        }

        public void Log(TraceEventType type, string message)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(type, 0, message);
        }
    }
}