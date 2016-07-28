using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class TypeInfo : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}