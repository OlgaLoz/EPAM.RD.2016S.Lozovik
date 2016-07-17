using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ReplicationInfo
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Master")]
        public ServiceDescription Master => (ServiceDescription)base["Master"];

        [ConfigurationProperty("Slave")]
        public ServiceDescription Slave => (ServiceDescription)base["Slave"];
    }
}
