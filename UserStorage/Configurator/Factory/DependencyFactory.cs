using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Storage.Interfaces.Interfaces;

namespace Configurator.Factory
{
    [Serializable]
    public class DependencyFactory : IFactory 
    {
        private readonly Dictionary<Type, string> types;

        public DependencyFactory(Dictionary<Type, string> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            this.types = types;
        }

        public T GetInstance<T>()
        {
            var strType = types[typeof(T)];
            if (string.IsNullOrEmpty(strType))
            {
                throw new InvalidOperationException($"There is no {nameof(T)} in factory.");
            }

            var type = Type.GetType(strType);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {strType}.");
            }

            T instance = (T)Activator.CreateInstance(type);
            if (instance?.GetType().GetCustomAttribute<SerializableAttribute>() == null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {strType}. Make it serializable.");
            }

            return instance;
        }
    }
}