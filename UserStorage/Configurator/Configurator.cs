using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Configurator.ReplicationInfo;
using Storage.Interfaces.Interfaces;

namespace Configurator
{
    public class Configurator
    {
        public IUserService masterService;
        private readonly List<IUserService> slaveServices = new List<IUserService>();
      
        public void Start()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("MSServices");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            var masterCollection = servicesSection.ServiceItems.Cast<ServiceDescription>()
                .Where(serviceItem => serviceItem.IsMaster).ToList();
            if (masterCollection.Count != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }
            masterService = CreateMaster(masterCollection[0]);
           ((IMaster)masterService).Load();

            var slaveCollection = servicesSection.ServiceItems.Cast<ServiceDescription>()
                .Where(serviceItem => !serviceItem.IsMaster).ToList();

            int i = 0;
            foreach (var serviceDescription in slaveCollection)
            {
                slaveServices.Add(CreateSlave(serviceDescription, ++i));
            }

        }

        public void End()
        {
            ((IMaster)masterService).Save();
        }

        private IMaster CreateMaster(ServiceDescription masterDescription )
        {
            AppDomain domain = AppDomain.CreateDomain("master");

            var generator = CreateInstance<IGenerator>(masterDescription.GeneratorType);
            var validator = CreateInstance<IValidator>(masterDescription.ValidatorType);
            var repository = CreateInstance<IRepository>(masterDescription.RepositoryType);

            var type = Type.GetType(masterDescription.Type);
            if (type == null)
                throw new ConfigurationErrorsException("Invalid type of master service.");
            var master = (IMaster)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, 
                BindingFlags.CreateInstance, null, 
                new object[] { validator, repository, generator },
                CultureInfo.InvariantCulture, null);

            if (master == null)
                throw new ConfigurationErrorsException("Unable to create master service.");

            return master;
        }

        private IUserService CreateSlave(ServiceDescription slaveDescription, int slaveIndex)
        {
            AppDomain domain = AppDomain.CreateDomain($"slave: {slaveIndex}");
            var types = SplitType(slaveDescription.Type);
            var slave = (IUserService)domain.CreateInstanceAndUnwrap(types[1], types[0], true,
               BindingFlags.CreateInstance, null,
               new object[] {masterService },
               CultureInfo.InvariantCulture, null);

            if (slave == null)
                throw new ConfigurationErrorsException("Unable to create slave service.");

            return slave;
        }

        private string[] SplitType(string src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            var result = src.Split(',');
            if (result.Length != 2)
            {
                throw new ConfigurationErrorsException("Unable to recognize type.");
            }

            return result;
        }

        private T CreateInstance<T>(string instanceType)
        {
            var type = Type.GetType(instanceType);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
                throw new ConfigurationErrorsException($"Unable to create instance of {instanceType}.");
            T instance = (T)Activator.CreateInstance(type);
            if (instance?.GetType().GetCustomAttribute<SerializableAttribute>()==null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {instanceType}. Make it serializable.");
            }
            return instance;
        }

    }
}
