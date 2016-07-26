using System;
using System.Diagnostics;
using Storage.Interfaces.Interfaces;

namespace Configurator.Logging
{
    public class DefaultLogger : MarshalByRefObject, ILogger
    {
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;
        public DefaultLogger()
        {
            traceSwitch = new BooleanSwitch("traceSwitch", "");
            traceSource = new TraceSource("trace");
        }

        public void Log(TraceEventType type, string message)
        {
            if (traceSwitch.Enabled)
            {
                traceSource.TraceEvent(type, 0, message);
            }
        }
    }
}