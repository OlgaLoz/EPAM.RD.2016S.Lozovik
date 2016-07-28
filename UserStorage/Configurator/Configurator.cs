using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Configurator.Logging;
using Configurator.ReplicationInfo;
using Storage.Interfaces.Interfaces;

namespace Configurator
{
    public class Configurator
    {
        private readonly List<IUserService> slaveServices = new List<IUserService>();
        private IUserService masterService;
      
        public IUserService Start()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("MSServices");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            var services = servicesSection.ServiceItems.Cast<ServiceDescription>().ToList();

            var masterCollection = services.Where(serviceItem => serviceItem.IsMaster).ToList();
         /*   if (masterCollection.Count != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }*/

            var slaveCollection = services.Where(serviceItem => !serviceItem.IsMaster).ToList();

            int i = 0;
            foreach (var serviceDescription in slaveCollection)
            {
                var slave = CreateSlave(serviceDescription, ++i);
                slaveServices.Add(slave);
            }

            foreach (var slaveService in slaveServices)
            {
                ((ISlave)slaveService).ListenForUpdate();
            }

            var slaveConnectionInfo = slaveCollection.Select(s => new IPEndPoint(IPAddress.Parse(s.IpAddress), s.Port)).ToList();
            masterService = CreateMaster(masterCollection[0], slaveConnectionInfo);
            ((IMaster)masterService).Load();
            return new Proxy(masterService, slaveServices);           
        }

        public void End()
        {
            ((IMaster)masterService).Save();
        }

        private IMaster CreateMaster(ServiceDescription masterDescription, IEnumerable<IPEndPoint> slaveConnectionInfo)
        {
            AppDomain domain = AppDomain.CreateDomain("master");

            var servicesSection = (DependencyConfigSection)ConfigurationManager.GetSection("Dependencies");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            var generator = CreateInstance<IGenerator>(servicesSection.Generator.Type);
            var validator = CreateInstance<IValidator>(servicesSection.Validator.Type);
            var repository = CreateInstance<IRepository>(servicesSection.Repository.Type);

            var type = Type.GetType(masterDescription.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException("Invalid type of master service.");
            }

            var master = (IMaster)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName, 
                true, 
                BindingFlags.CreateInstance, 
                null, 
                new object[] { validator, repository, generator, slaveConnectionInfo, GlobalLogger.Logger },
                CultureInfo.InvariantCulture,
                null);

            if (master == null)
            {
                throw new ConfigurationErrorsException("Unable to create master service.");
            }

            return master;
        }

        private ISlave CreateSlave(ServiceDescription slaveDescription, int slaveIndex)
        {
            AppDomain domain = AppDomain.CreateDomain($"slave#{slaveIndex}");

            var servicesSection = (DependencyConfigSection)ConfigurationManager.GetSection("Dependencies");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            var repository = CreateInstance<IRepository>(servicesSection.Repository.Type);
            var type = Type.GetType(slaveDescription.Type);
            if (type == null)
                throw new ConfigurationErrorsException("Invalid type of slave service.");
            var slave = (ISlave)domain.CreateInstanceAndUnwrap(type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] {new IPEndPoint(IPAddress.Parse(slaveDescription.IpAddress),slaveDescription.Port ), repository, GlobalLogger.Logger},
                CultureInfo.InvariantCulture, 
                null);

            if (slave == null)
                throw new ConfigurationErrorsException("Unable to create slave service.");

            return slave;
        }

        private T CreateInstance<T>(string instanceType)
        {
            var type = Type.GetType(instanceType);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
                throw new ConfigurationErrorsException($"Unable to create instance of {instanceType}.");
            T instance = (T)Activator.CreateInstance(type);
            if (instance?.GetType().GetCustomAttribute<SerializableAttribute>() == null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {instanceType}. Make it serializable.");
            }
            return instance;
        }

    }
}
