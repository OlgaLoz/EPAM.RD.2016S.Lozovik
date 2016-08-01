using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class AddressConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Addresses")]
        public AddressCollection ServiceItems => (AddressCollection)base["Addresses"];
    }
}