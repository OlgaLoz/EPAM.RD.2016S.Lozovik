using System.Diagnostics;

namespace Storage.Interfaces.Logger
{
    public interface ILogger
    {
        void Log(TraceEventType type, string message);
    }
}