﻿using System.Configuration;

namespace Configurator.ReplicationInfo
{
    [ConfigurationCollection(typeof(ServiceDescription), AddItemName = "add")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public ServiceDescription this[int index] => (ServiceDescription)BaseGet(index);

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceDescription();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceDescription)element).Port;
        }
    }
}