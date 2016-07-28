using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class ServiceDescription : ConfigurationElement
    {
        [ConfigurationProperty("isMaster", DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool IsMaster
        {
            get { return (bool)base["isMaster"]; }
            set { base["isMaster"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "",  IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 0, IsKey = true, IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "127.0.0.1", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }
    }
}
