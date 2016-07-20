using System;
using System.Collections.Generic;
using System.Configuration;
using Configurator.ReplicationInfo;
using FibbonacciGenerator;
using Storage.Entities.UserInfo;
using Storage.Interfaces;
using Storage.Loader;
using Storage.Service;

namespace Configurator
{
    public class Configurator
    {
        private IUserService masterService;
        private List<IUserService> slaveServices;

        public void Start()
        {
            ServicesConfigSection servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("Services");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            if (servicesSection.Master.Count != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }
            
            masterService = new Master(new List<Func<User, bool>>(), new Repository(), new FibonacciGenerator());
            ((IMaster)masterService).Load();

            slaveServices = new List<IUserService>();

            for (int i = 0; i < servicesSection.Slave.Count; i++)
            {
                IUserService slaveService = new Slave((IMaster)masterService);
                slaveServices.Add(slaveService);
            }
        }

        public void End()
        {
            ((IMaster)masterService).Save();
        }
    }
}
