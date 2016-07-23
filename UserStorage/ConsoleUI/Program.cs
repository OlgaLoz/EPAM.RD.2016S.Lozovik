using System;
using CustomConfigurator = Configurator.Configurator;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomConfigurator configurator = new CustomConfigurator();
            configurator.Start();

            Console.ReadLine();
        }
    }
}
