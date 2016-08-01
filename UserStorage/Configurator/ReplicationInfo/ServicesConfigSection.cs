using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServicesCollection ServiceItems => (ServicesCollection)base["Services"];
    }
}
