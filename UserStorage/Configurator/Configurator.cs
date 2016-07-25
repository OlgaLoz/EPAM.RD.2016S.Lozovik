﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Configurator.ReplicationInfo;
using Storage.Interfaces.Interfaces;

namespace Configurator
{
    public class Configurator
    {
        public IUserService masterService;
        private readonly List<ISlave> slaveServices = new List<ISlave>();
      
        public void Start()
        {
            var servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("MSServices");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }
            var services = servicesSection.ServiceItems.Cast<ServiceDescription>().ToList();

            var masterCollection = services.Where(serviceItem => serviceItem.IsMaster).ToList();
            if (masterCollection.Count != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }

            var slaveCollection = services.Where(serviceItem => !serviceItem.IsMaster).ToList();

            int i = 0;
            foreach (var serviceDescription in slaveCollection)
            {
                var slave = CreateSlave(serviceDescription, ++i);
                slaveServices.Add(slave);
                slave.InitializeCollection();
                slave.ListenForUpdate();
            }

            var slaveConnectionInfo = slaveCollection.Select(s => new IPEndPoint(IPAddress.Parse(s.IpAddress), s.Port)).ToList();
            masterService = CreateMaster(masterCollection[0], slaveConnectionInfo);
            ((IMaster)masterService).Load();
            
        }

        public void End()
        {
            ((IMaster)masterService).Save();
        }

        private IMaster CreateMaster(ServiceDescription masterDescription , IEnumerable<IPEndPoint> slaveConnectionInfo )
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
                new object[] { validator, repository, generator, slaveConnectionInfo },
                CultureInfo.InvariantCulture, null);

            if (master == null)
                throw new ConfigurationErrorsException("Unable to create master service.");

            return master;
        }

        private ISlave CreateSlave(ServiceDescription slaveDescription, int slaveIndex)
        {
            AppDomain domain = AppDomain.CreateDomain($"slave: {slaveIndex}");
            var type = Type.GetType(slaveDescription.Type);
            if (type == null)
                throw new ConfigurationErrorsException("Invalid type of slave service.");
            var slave = (ISlave)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true,
               BindingFlags.CreateInstance, null,
               new object[] {new IPEndPoint(IPAddress.Parse(slaveDescription.IpAddress),slaveDescription.Port ), },
               CultureInfo.InvariantCulture, null);

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
            if (instance?.GetType().GetCustomAttribute<SerializableAttribute>()==null)
            {
                throw new ConfigurationErrorsException($"Unable to create instance of {instanceType}. Make it serializable.");
            }
            return instance;
        }

    }
}
