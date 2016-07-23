using System;
using Storage.Interfaces.Entities.UserInfo;
using CustomConfigurator = Configurator.Configurator;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
        /*    var ms = new Master(new UserValidator(), new CustomRepository(), new FibonacciGenerator());
        //  ms.Add(new User {FirstName = "sdfv", Visas = new[] {new Visa {Country = "dffv"}}});
        //    ms.Save();
            ms.Load();*/
             CustomConfigurator configurator = new CustomConfigurator();
            configurator.Start();
            configurator.masterService.Add(new User() {FirstName = "sdcf", Visas = new [] {new Visa {Country = "asd"}, }});
            configurator.End();
            Console.ReadLine();
        }
    }
}
