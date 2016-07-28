using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class DependencyConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("generator")]
        public TypeInfo Generator=> (TypeInfo)base["generator"];

        [ConfigurationProperty("validator")]
        public TypeInfo Validator => (TypeInfo)base["validator"];

        [ConfigurationProperty("repository")]
        public TypeInfo Repository => (TypeInfo)base["repository"];
    }
}