using Storage.Interfaces.Interfaces;

namespace Configurator.Logging
{
    public class GlobalLogger 
    {
        public static ILogger Logger { get; internal set; }

        static GlobalLogger()
        {
            Logger = new DefaultLogger();
        }
    }
}