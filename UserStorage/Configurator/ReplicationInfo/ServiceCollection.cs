using System.Configuration;

namespace Configurator.ReplicationInfo
{
    public class ServiceCollection
    {
        [ConfigurationCollection(typeof(ServiceDescription), AddItemName = "add")]
        public class ServicesCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new ServiceDescription();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((ServiceDescription)element).Port;
            }

            public ServiceDescription this[int index] => (ServiceDescription)BaseGet(index);
        }
    }
}