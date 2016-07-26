using System.Diagnostics;

namespace Storage.Interfaces.Interfaces
{
    public interface ILogger
    {
        void Log(TraceEventType type, string message);
    }
}