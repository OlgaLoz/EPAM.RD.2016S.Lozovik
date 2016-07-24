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

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["poipAddressrt"] = value; }
        }

        [ConfigurationProperty("generatorType", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string GeneratorType
        {
            get { return (string)base["generatorType"]; }
            set { base["generatorType"] = value; }
        }

        [ConfigurationProperty("validatorType", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string ValidatorType
        {
            get { return (string)base["validatorType"]; }
            set { base["validatorType"] = value; }
        }

        [ConfigurationProperty("repositoryType", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string RepositoryType
        {
            get { return (string)base["repositoryType"]; }
            set { base["repositoryType"] = value; }
        }
    }
}
