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
using Storage.Interfaces.ServiceInfo;
using Storage.Interfaces.Services;
using Storage.Interfaces.Validator;
using WcfLibrary;

namespace Configurator
{
    public class Configurator
    {
        private readonly List<IWcfHelper> slaveServices = new List<IWcfHelper>();
        private IWcfHelper masterService;
      
        public void Start()
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

            foreach (var serviceDescription in slaveCollection)
            {
                Dictionary<Type, InstanceInfo> slaveTypes = new Dictionary<Type, InstanceInfo>
                {
                   { typeof(IRepository), new InstanceInfo { Type = dependencySection.Repository.Type } },
                   { typeof(ILogger), new InstanceInfo { Type = dependencySection.Logger.Type } },
                   { typeof(IReceiver), new InstanceInfo { Type = dependencySection.Receiver.Type, Params = new object[] { new IPEndPoint(IPAddress.Parse(serviceDescription.IpAddress), serviceDescription.Port) } } }
                   };
                var slave = CreateService(serviceDescription, new DependencyFactory(slaveTypes));
                slaveServices.Add(slave);
            }

            var slaveConnectionInfo = slaveCollection.Select(s => new IPEndPoint(IPAddress.Parse(s.IpAddress), s.Port)).ToList();
            Dictionary<Type, InstanceInfo> types = new Dictionary<Type, InstanceInfo>
            {
                { typeof(IGenerator), new InstanceInfo { Type = dependencySection.Generator.Type } },
                { typeof(IValidator), new InstanceInfo { Type = dependencySection.Validator.Type } },
                { typeof(IRepository), new InstanceInfo { Type = dependencySection.Repository.Type } },
                { typeof(ILogger), new InstanceInfo { Type = dependencySection.Logger.Type } },
                { typeof(ISender), new InstanceInfo { Type = dependencySection.Sender.Type, Params = new object[] { slaveConnectionInfo } } }
            };
            masterService = CreateService(masterCollection[0], new DependencyFactory(types));                   
         ////   return new Proxy(masterService, slaveServices);                
        }

        public void End()
        {
            masterService.Close();
            foreach (var slaveService in slaveServices)
            {
                slaveService.Close();
            }
        }

        private IWcfHelper CreateService(ServiceDescription masterDescr, DependencyFactory factory)
        {
            AppDomain domain = AppDomain.CreateDomain(masterDescr.Host.Substring(masterDescr.Host.LastIndexOf('/') + 1));

            var type = Type.GetType(masterDescr.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException("Invalid type of service.");
            }

            var service = (IUserService)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { factory },
                CultureInfo.InvariantCulture,
                null);

            if (service == null)
            {
                throw new ConfigurationErrorsException("Unable to create service.");
            }

            var host = (IWcfHelper)domain.CreateInstanceAndUnwrap(
                typeof(WcfHelper).Assembly.FullName,
                typeof(WcfHelper).FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { service },
                CultureInfo.InvariantCulture,
                null);

            host.Open(masterDescr.Host);
            return host;
        }
    }
}
