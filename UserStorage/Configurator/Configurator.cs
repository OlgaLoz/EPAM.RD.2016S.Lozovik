using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Configurator.Factory;
using Configurator.ReplicationInfo;
using Storage.Interfaces.Generator;
using Storage.Interfaces.Logger;
using Storage.Interfaces.Network;
using Storage.Interfaces.Repository;
using Storage.Interfaces.Services;
using Storage.Interfaces.Validator;

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

            var dependencySection = (DependencyConfigSection)ConfigurationManager.GetSection("Dependencies");
            if (dependencySection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            var services = servicesSection.ServiceItems.Cast<ServiceDescription>().ToList();
            var masterCollection = services.Where(serviceItem => serviceItem.IsMaster).ToList();
            var slaveCollection = services.Where(serviceItem => !serviceItem.IsMaster).ToList();

            int i = 0;
            foreach (var serviceDescription in slaveCollection)
            {
                var slave = CreateSlave(serviceDescription, dependencySection, ++i);
                slaveServices.Add(slave);
                (slave as IListener)?.ListenForUpdate();
            }

            var slaveConnectionInfo = slaveCollection.Select(s => new IPEndPoint(IPAddress.Parse(s.IpAddress), s.Port)).ToList();
            masterService = CreateMaster(masterCollection[0], dependencySection, slaveConnectionInfo);
            (masterService as ILoader)?.Load();
            return new Proxy(masterService, slaveServices);           
        }

        public void End()
        {
            (masterService as ILoader)?.Save();
        }

        private IUserService CreateMaster(ServiceDescription masterDescription, DependencyConfigSection servicesSection, IEnumerable<IPEndPoint> slaves)
        {
            AppDomain domain = AppDomain.CreateDomain("master");

            var type = Type.GetType(masterDescription.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException("Invalid type of master service.");
            }

            Dictionary<Type, InstanceInfo> types = new Dictionary<Type, InstanceInfo>
            {
                { typeof(IGenerator), new InstanceInfo { Type = servicesSection.Generator.Type } },
                { typeof(IValidator), new InstanceInfo { Type = servicesSection.Validator.Type } },
                { typeof(IRepository), new InstanceInfo { Type = servicesSection.Repository.Type } },
                { typeof(ILogger), new InstanceInfo { Type = servicesSection.Logger.Type } },
                { typeof(ISender), new InstanceInfo { Type = servicesSection.Sender.Type, Params = new object[] { slaves } } }
            };

            var factory = new DependencyFactory(types);
            var master = (IUserService)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { factory, slaves },
                CultureInfo.InvariantCulture,
                null);

            if (master == null)
            {
                throw new ConfigurationErrorsException("Unable to create master service.");
            }

            return master;
        }

        private IUserService CreateSlave(ServiceDescription slaveDescription, DependencyConfigSection servicesSection, int slaveIndex)
        {
            AppDomain domain = AppDomain.CreateDomain($"slave#{slaveIndex}");

            var type = Type.GetType(slaveDescription.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException("Invalid type of slave service.");
            }

            Dictionary<Type, InstanceInfo> types = new Dictionary<Type, InstanceInfo>
            {
                { typeof(IRepository), new InstanceInfo { Type = servicesSection.Repository.Type } },
                { typeof(ILogger), new InstanceInfo { Type = servicesSection.Logger.Type } },
                { typeof(IReceiver), new InstanceInfo { Type = servicesSection.Receiver.Type, Params = new object[] { new IPEndPoint(IPAddress.Parse(slaveDescription.IpAddress), slaveDescription.Port) } } }
            };

            var factory = new DependencyFactory(types);
            var slave = (IUserService)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { factory },
                CultureInfo.InvariantCulture,
                null);

            if (slave == null)
            {
                throw new ConfigurationErrorsException("Unable to create slave service.");
            }

            return slave;
        }
    }
}
