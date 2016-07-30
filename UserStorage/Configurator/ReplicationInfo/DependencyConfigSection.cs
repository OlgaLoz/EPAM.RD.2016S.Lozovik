using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class DependencyConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("generator")]
        public TypeInfo Generator => (TypeInfo)base["generator"];

        [ConfigurationProperty("validator")]
        public TypeInfo Validator => (TypeInfo)base["validator"];

        [ConfigurationProperty("repository")]
        public TypeInfo Repository => (TypeInfo)base["repository"];

        [ConfigurationProperty("logger")]
        public TypeInfo Logger => (TypeInfo)base["logger"];

        [ConfigurationProperty("sender")]
        public TypeInfo Sender => (TypeInfo)base["sender"];

        [ConfigurationProperty("receiver")]
        public TypeInfo Receiver => (TypeInfo)base["receiver"];
    }
}