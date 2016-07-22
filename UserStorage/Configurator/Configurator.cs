using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Configurator.ReplicationInfo;
using FibbonacciGenerator.Interface;
using Storage.Interfaces;


namespace Configurator
{
    public class Configurator
    {
        private IUserService masterService;
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
          
            var type = Type.GetType(masterDescription.GeneratorType);
            if (type?.GetInterface("IGenerator") == null)
                throw new ConfigurationErrorsException("Unable to create generator.");
            var generator = (IGenerator)Activator.CreateInstance(type);

            type = Type.GetType(masterDescription.ValidatorType);
            if (type?.GetInterface("IValidator") == null)
                throw new ConfigurationErrorsException("Unable to create validator.");
            var validator = (IValidator)Activator.CreateInstance(type);

            type = Type.GetType(masterDescription.RepositoryType);
            if (type?.GetInterface("IRepository") == null)
                throw new ConfigurationErrorsException("Unable to create repository.");
            var repository = (IRepository)Activator.CreateInstance(type);

            type = Type.GetType(masterDescription.Type);

            var types = SplitType(masterDescription.Type);
            var master = (IMaster)domain.CreateInstanceAndUnwrap(types[1], types[0], true, 
                BindingFlags.CreateInstance, null, 
                new object[] { validator, repository, generator },
                CultureInfo.InvariantCulture, null);


       /*     var ctor = type?.GetConstructor(new[] { typeof(IValidator), typeof(IRepository), typeof(IGenerator) });
            var master = (IMaster)ctor?.Invoke(new object[] { validator, repository, generator });*/
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

            /* var type = Type.GetType(slaveDescription.Type);
             var slaveCtor = type?.GetConstructor(new[] { typeof(IMaster) });
             var slave = (IUserService)slaveCtor?.Invoke(new object[] { masterService });*/
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
                throw new ConfigurationErrorsException("Enable to recognize type.");
            }

            return result;
        }

    }
}
