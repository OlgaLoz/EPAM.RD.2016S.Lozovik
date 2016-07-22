using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection.ServicesCollection ServiceItems
        {
            get { return (ServiceCollection.ServicesCollection)base["Services"]; }
        }
    }
}
