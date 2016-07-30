using System;
using System.Diagnostics;
using Storage.Interfaces.Logger;

namespace Configurator.Logging
{
    [Serializable]
    public class DefaultLogger : ILogger
    {
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        public DefaultLogger()
        {
            traceSwitch = new BooleanSwitch("traceSwitch", string.Empty);
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