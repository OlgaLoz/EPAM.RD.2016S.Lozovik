using System;
using System.IO;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;
using Storage.Service;
using CustomConfigurator = Configurator.Configurator;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
        /*    var ms = new Master(new UserValidator(), new CustomRepository(), new FibonacciGenerator());
        //  ms.Add(new User {FirstName = "sdfv", Visas = new[] {new Visa {Country = "dffv"}}});
        //    ms.Save();
            ms.Load();*/
             CustomConfigurator configurator = new CustomConfigurator();
             IUserService us = configurator.Start();
            for (int i = 0; i < 30; i++)
            {
                 us.Add(new User
                 {
                     FirstName = $"{i}",
                     Visas = new[]
                     {
                         new Visa
                         {
                             Country = "asd"
                         }
                     }
                 });
            }

            configurator.End();
            File.Delete("users.xml");
            Console.ReadLine();
        }
    }
}