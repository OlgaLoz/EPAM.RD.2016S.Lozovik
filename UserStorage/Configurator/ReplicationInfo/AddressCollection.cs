using System.Configuration;

namespace Configurator.ReplicationInfo
{
    [ConfigurationCollection(typeof(AddressElement), AddItemName = "add")]
    public class AddressCollection : ConfigurationElementCollection
    {
        public AddressElement this[int index] => (AddressElement)BaseGet(index);

        protected override ConfigurationElement CreateNewElement()
        {
            return new AddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AddressElement)element).Port;
        }
    }
}