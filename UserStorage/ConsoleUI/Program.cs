using System;
using CustomConfigurator = Configurator.Configurator;

namespace ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomConfigurator configurator = new CustomConfigurator();
            configurator.Start();
            Console.ReadLine();
            configurator.End();
        }
    }
}