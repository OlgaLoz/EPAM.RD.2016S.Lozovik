using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Storage.Interfaces.Factory;

namespace Configurator.Factory
{
    [Serializable]
    public class DependencyFactory : IFactory 
    {
        private readonly Dictionary<Type, InstanceInfo> types;

        public DependencyFactory(Dictionary<Type, InstanceInfo> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            this.types = types;
        }

        public T GetInstance<T>()
        {
            var typeInfo = types[typeof(T)];
            if (string.IsNullOrEmpty(typeInfo.Type))
            {
                throw new InvalidOperationException($"There is no {nameof(T)} in factory.");
            }

            var type = Type.GetType(typeInfo.Type);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(typeInfo.Params.Select(t => t?.GetType()).ToArray()) == null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {typeInfo}.");
            }

            T instance = (T)Activator.CreateInstance(type, typeInfo.Params);
            if (instance == null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {typeInfo}.");
            }

            return instance;
        }
    }
}