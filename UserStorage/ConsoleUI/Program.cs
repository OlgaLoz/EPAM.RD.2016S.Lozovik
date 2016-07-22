using System;
using System.Collections.Generic;
using Storage.Entities.ServiceState;
using Storage.Entities.UserInfo;
using Storage.Serializer;
using CustomConfigurator = Configurator.Configurator;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            //var storage = new LoggerDecorator(new UserService
            //      (new Master(new List<Func<User, bool>>()), new Repository(), new Fibbonacci()));
            // // storage.Load();
            //  storage.Add(new User {FirstName = "qwerty"});
            //storage.Save();

            //ServicesConfigSection servicesSection = (ServicesConfigSection)ConfigurationManager.GetSection("Services");
            CustomConfigurator configurator = new CustomConfigurator();
            configurator.Start();

            Console.ReadLine();
        }
    }
}
