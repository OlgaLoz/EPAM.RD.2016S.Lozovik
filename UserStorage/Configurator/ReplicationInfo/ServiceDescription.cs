using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ReplicationInfo
{
    public class ServiceDescription : ConfigurationElement
    {
        [ConfigurationProperty("count", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int Count
        {
            get { return ((int)(base["count"])); }
            set { base["count"] = value; }
        }
    }
}
