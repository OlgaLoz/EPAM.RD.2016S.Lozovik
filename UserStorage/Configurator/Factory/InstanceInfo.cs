using System;

namespace Configurator.Factory
{
    [Serializable]
    public class InstanceInfo
    {
        public string Type { get; set; }

        public object[] Params { get; set; } = { };
    }
}