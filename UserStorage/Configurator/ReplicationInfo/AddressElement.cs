using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class AddressElement : ConfigurationElement
    {
        [ConfigurationProperty("port", DefaultValue = 0, IsKey = true, IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "127.0.0.1", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }
    }
}