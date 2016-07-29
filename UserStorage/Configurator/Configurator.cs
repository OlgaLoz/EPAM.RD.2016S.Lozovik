using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Configurator.Factory;
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
            var slaveCollection = services.Where(serviceItem => !serviceItem.IsMaster).ToList();

            int i = 0;
            foreach (var serviceDescription in slaveCollection)
            {
                var slave = CreateSlave(serviceDescription, ++i);
                slaveServices.Add(slave);
                slave.ListenForUpdate();
            }

            var slaveConnectionInfo = slaveCollection.Select(s => new IPEndPoint(IPAddress.Parse(s.IpAddress), s.Port)).ToList();
            masterService = CreateMaster(masterCollection[0], slaveConnectionInfo);
            ((IMaster)masterService).Load();
            return new Proxy(masterService, slaveServices);           
        }

        public void End()
        {
            var master = masterService as IMaster;
            master?.Save();
        }

        private IMaster CreateMaster(ServiceDescription masterDescription, IEnumerable<IPEndPoint> slaveConnectionInfo)
        {
            AppDomain domain = AppDomain.CreateDomain("master");

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
                new object[] { CreateFactory(), slaveConnectionInfo },
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

            var type = Type.GetType(slaveDescription.Type);
            if (type == null)
            {
                throw new ConfigurationErrorsException("Invalid type of slave service.");
            }

            var slave = (ISlave)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { new IPEndPoint(IPAddress.Parse(slaveDescription.IpAddress), slaveDescription.Port), CreateFactory() },
                CultureInfo.InvariantCulture,
                null);

            if (slave == null)
            {
                throw new ConfigurationErrorsException("Unable to create slave service.");
            }

            return slave;
        }

        private IFactory CreateFactory()
        {
            var servicesSection = (DependencyConfigSection)ConfigurationManager.GetSection("Dependencies");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            Dictionary<Type, string> types = new Dictionary<Type, string>
            {
                { typeof(IGenerator), servicesSection.Generator.Type },
                { typeof(IValidator), servicesSection.Validator.Type },
                { typeof(IRepository), servicesSection.Repository.Type },
                { typeof(ILogger), servicesSection.Logger.Type }
            };
            return new DependencyFactory(types);
        } 
    }
}
