using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection.ServicesCollection ServiceItems => (ServiceCollection.ServicesCollection)base["Services"];
    }
}
