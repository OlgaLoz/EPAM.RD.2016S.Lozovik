using System;
using System.Collections.Generic;
using System.Configuration;
using Configurator.ReplicationInfo;
using FibbonacciGenerator;
using Storage.Loader;
using Storage.Service;
using Storage.Strategy;
using Storage.UserInfo;

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
            
            Master master = new Master(new List<Func<User, bool>>());
            masterService = new UserService(master, new Loader(), new Fibbonacci());
            ((UserService)masterService).Load();

            slaveServices = new List<IUserService>();

            for (int i = 0; i < servicesSection.Slave.Count; i++)
            {
                Slave slave = new Slave(master);
                IUserService slaveService = new UserService(slave, new Loader(), null);
                ((UserService)slaveService).Load();
                slaveServices.Add(slaveService);
            }
        }

        public void End()
        {
            ((UserService)masterService).Save();
        }
    }
}
