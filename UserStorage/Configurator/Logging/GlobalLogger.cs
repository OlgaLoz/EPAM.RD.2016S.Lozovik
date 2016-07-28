using Storage.Interfaces.Interfaces;

namespace Configurator.Logging
{
    public class GlobalLogger 
    {
        static GlobalLogger()
        {
            Logger = new DefaultLogger();
        }

        public static ILogger Logger { get; internal set; }

    }
}